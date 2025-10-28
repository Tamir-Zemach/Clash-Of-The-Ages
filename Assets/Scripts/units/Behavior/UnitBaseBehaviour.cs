using System;
using System.Collections;
using BackEnd.Base_Classes;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace units.Behavior
{
    public class UnitBaseBehaviour : InGameObject
    {
        public delegate void AttackDelegate(GameObject target, int strength);
        public event AttackDelegate OnAttack;

        public event Action OnInitialized;
        public event Action OnDying;

        public delegate void Spawned(Renderer renderer);
        public static event Spawned OnUnFriendlySpawned;

        private UnitRayCaster _raycaster;
        private UnitHealthManager _healthManager;
        private UnitMovement _movement;
        private GameObject _enemyBase;
        private GameObject _currentTarget;
        private Animator _animator;
        private ManagedCoroutine _attackCoroutine;
        private Collider _col;
        private Renderer _renderer;

        private bool _isAttacking = false;
        private bool _isDying;
        private float _defaultSpeed;
        
        public UnitData Unit { get; private set; }

        public bool IsAttacking => _isAttacking;
        public GameObject GetAttackTarget() => _currentTarget;

        private void Awake()
        {
            GetComponents();
            SubscribeToRayCaster();
            _healthManager.OnDying += Dying;
        }

        private void GetComponents()
        {
            _col = GetComponent<Collider>();
            _animator = GetComponentInChildren<Animator>();
            _renderer = GetComponentInChildren<Renderer>();
            _raycaster = GetComponent<UnitRayCaster>();
            _healthManager = GetComponent<UnitHealthManager>();
            _movement = GetComponent<UnitMovement>();
        }

        private void SubscribeToRayCaster()
        {
            _raycaster.OnFriendlyDetection += HandleFriendlyDetection;
            _raycaster.OnEnemyDetection += HandleEnemyDetection;
            _raycaster.OnNoDetection += HandleNoDetection;
        }

        public void Initialize(UnitData unitData, Transform destination = null)
        {
            Unit = unitData;
            _defaultSpeed = Unit.Speed;

            SetDestination(destination);
            _movement.ResumeMovement(_defaultSpeed, 0.2f);
            InvokeSpawned();
        }

        private void InvokeSpawned()
        {
            OnInitialized?.Invoke();

            if (!Unit.IsFriendly)
            {
                OnUnFriendlySpawned?.Invoke(_renderer);
            }
        }

        private void SetDestination(Transform destination = null)
        {
            if (destination == null)
            {
                _enemyBase = GameObject.FindGameObjectWithTag(Unit.OppositeBaseTag);
                _movement.SetDestination(_enemyBase.transform);
            }
            else
            {
                _movement.SetDestination(destination);
                print(destination.name);
            }
            
        }

        private void Update()
        {
            if (_isDying || !GameStates.Instance.GameIsPlaying) return;
            ResetAttackStateIfNeeded();
        }

        private void HandleEnemyDetection(GameObject target)
        {
            _movement.StopMovement();
            if (_isAttacking) return;
            _isAttacking = true;
            Attack(target);
        }

        private void HandleFriendlyDetection(GameObject obj)
        {
            _movement.StopMovement();
        }
        
        private void HandleNoDetection()
        {
            _isAttacking = false;

            if (!_isDying)
            {
                _movement.ResumeMovement(_defaultSpeed);
            }
        }

        private void ResetAttackStateIfNeeded()
        {
            if (_attackCoroutine != null && !_movement.IsStopped && !_raycaster.IsDetecting)
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

        private IEnumerator AttackAction(float initialAttackDelay, GameObject target)
        {
            yield return new WaitForSeconds(initialAttackDelay);

            if (_animator == null)
            {
                var randomStrength = SetRandomStrength();
                OnAttack?.Invoke(target, randomStrength);
            }

            _isAttacking = false;

            // Resume movement if target is gone
            if (target == null || !target.activeInHierarchy)
            {
                _movement.ResumeMovement(_defaultSpeed);
            }
        }
        private void Dying()
        {
            _isDying = true;
            _movement.StopMovement();
            _col.enabled = false;

            _attackCoroutine?.Stop();
            _attackCoroutine = null;
            _isAttacking = false; 
            OnDying?.Invoke();
        }

        private int SetRandomStrength()
        {
            return Random.Range(Unit.MinStrength, Unit.MaxStrength);
        }

        #region GameLifecycle

        protected override void HandlePause()
        {
            _movement.StopMovement();
        }

        protected override void HandleResume()
        {
            _movement.ResumeMovement(_defaultSpeed);
        }

        protected override void HandleGameEnd() { }

        protected override void HandleGameReset() { }

        private void OnDestroy()
        {
            _healthManager.OnDying -= Dying;
        }

        #endregion
    }
}