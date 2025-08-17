using System;
using System.Collections;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using Managers;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace units.Behavior
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitBaseBehaviour : InGameObject
    {
        public delegate void AttackDelegate(GameObject target, int strength);
        public event AttackDelegate OnAttack;

        public event Action OnInitialized;
        public event Action OnDying;

        public delegate void Spawned(Renderer renderer);
        public static event Spawned OnSpawned;

        private NavMeshAgent _agent;
        private UnitHealthManager _healthManager;
        private GameObject _enemyBase;
        private GameObject _currentTarget;
        private Animator _animator;
        private ManagedCoroutine _attackCoroutine;
        private Collider _col;
        private Renderer _renderer;

        private bool _isAttacking = false;
        private bool _isDying;


        public UnitData Unit { get; private set; }

        public bool IsAttacking => _isAttacking;

        public GameObject GetAttackTarget() => _currentTarget;

        private void Awake()
        {
            _col = GetComponent<Collider>();
            _animator = GetComponentInChildren<Animator>();
            _renderer = GetComponentInChildren<Renderer>();
            _agent = GetComponent<NavMeshAgent>();
            _healthManager = GetComponent<UnitHealthManager>();
            _healthManager.OnDying += Dying;
        }
        

        public void Initialize(UnitData unitData, Transform destination = null)
        {
            Unit = unitData;
            
            SetDestination(destination);
            _agent.speed = Unit.Speed;
            
            OnInitialized?.Invoke();
            
            if (!Unit.IsFriendly)
            {
                OnSpawned?.Invoke(_renderer);
            }

        }

        private void SetDestination(Transform destination)
        {
            if (destination == null)
            {
                _enemyBase = GameObject.FindGameObjectWithTag(Unit.OppositeBaseTag);
                _agent.destination = _enemyBase.transform.position;
            }
            else
            {
                _agent.destination = destination.position;
            }
            
        }


        private void Update()
        {
            if (_isDying || !GameStates.Instance.GameIsPlaying) return;

            CheckForEnemyUnit();
            CheckForFriendlyUnit();
        }


        private void CheckForEnemyUnit()
        {
            _agent.isStopped = false; 

            if (Physics.BoxCast(transform.position,
                    Unit.boxSize,
                    transform.forward,
                    out var hitInfo,
                    Quaternion.identity,
                    Unit.Range,
                    Unit.OppositeUnitMask))
            {
                GameObject obj = hitInfo.transform.gameObject;

                if (obj.CompareTag(Unit.OppositeUnitTag) || obj.CompareTag(Unit.OppositeBaseTag))
                {
                    HandleEnemyDetection(obj);
                }
            }

            ResetAttackStateIfNeeded();
        }
        private void HandleEnemyDetection(GameObject target)
        {
            _agent.isStopped = true;
            if (!_isAttacking)
            {
                _isAttacking = true;
                Attack(target);
            }
        }

        private void CheckForFriendlyUnit()
        {
            if (Physics.BoxCast(transform.position,
                    Unit.boxSize, transform.forward,
                    out var hitInfo,
                    Quaternion.identity,
                    Unit.RayLengthForFriendlyUnit))
            {
                GameObject obj = hitInfo.transform.gameObject;
                if (obj.CompareTag(Unit.FriendlyUnitTag))
                {
                    _agent.isStopped = true;
                }
            }
        }


        private void ResetAttackStateIfNeeded()
        {
            if (_attackCoroutine != null && !_agent.isStopped)
            {
                _attackCoroutine.Stop();
                _attackCoroutine = null;
                _isAttacking = false;
            }
        }
        private void Attack(GameObject target)
        {
            _currentTarget = target;
            _attackCoroutine = CoroutineManager.Instance.StartManagedCoroutine(AttackAction(Unit.InitialAttackDelay, target));
        }

        IEnumerator AttackAction(float initialAttackDelay, GameObject target)
        {
            yield return new WaitForSeconds(initialAttackDelay);

            if (_animator == null)
            {
                var randomStrength = SetRandomStrength();
                OnAttack?.Invoke(target, randomStrength);
            }

            _isAttacking = false;
        }
        private void Dying()
        {
            _isDying = true;
            _agent.isStopped = true;
            _col.enabled = false;

            _attackCoroutine?.Stop();
            _attackCoroutine = null;
            OnDying?.Invoke();
        }

        private int SetRandomStrength()
        {
            return Random.Range(Unit.MinStrength,  Unit.MaxStrength);
        }
        
        
        

        #region GameLifcyle

        protected override void HandlePause()
        {
            _attackCoroutine?.Pause();
            _agent.isStopped = true;
        }

        protected override void HandleResume()
        {
            _attackCoroutine?.Resume();
            _agent.isStopped = false;
        }

        protected override void HandleGameEnd()
        {
            _attackCoroutine?.Stop();
            _attackCoroutine = null;
        }

        protected override void HandleGameReset()
        {
            _attackCoroutine?.Stop();
            _attackCoroutine = null;
        }
        
        private void OnDestroy()
        {
            
            _healthManager.OnDying -= Dying;
        }

        #endregion

    }
}



