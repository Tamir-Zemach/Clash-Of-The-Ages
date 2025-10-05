using System;
using System.Collections;
using BackEnd.Data__ScriptableOBj_;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace turrets
{
    public class TurretBaseBehavior : InGameObject
    {
        public Action<Quaternion> OnSeeingEnemy;
        public Action OnSeeingNothing;
        public Action OnAttack;

        [Tooltip("Transform from where bullets will be instantiated.")]
        [SerializeField] private Transform _bulletSpawnPos;
        
        [Tooltip("Projectile prefab to spawn when attacking.")]
        public GameObject BulletPrefab;
                
        [Tooltip("Gfx that looks at the approaching enemies.")]
        public GameObject MovableGfxPrefab;
        
        
        private Vector3 _origin;
        private Vector3 _direction;
        private Quaternion _rotation;
        private bool _isAttacking;
        private ManagedCoroutine _attackCoroutine;

        public TurretData Turret { get; private set; }

        public Vector3 Origin => _origin;
        public Vector3 Direction => _direction;
        public Quaternion Rotation => _rotation;

        public void Initialize(TurretData turretData,Transform originTransform, GameObject oppositeBase = null)
        {
            Turret = turretData;
            
            _origin = originTransform.position;
            _direction = oppositeBase != null ? (oppositeBase.transform.position - transform.position).normalized : originTransform.forward;
            _rotation = originTransform.rotation;
        }

        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            CheckForEnemies();
        }

        private void CheckForEnemies()
        {
            if (Physics.BoxCast(_origin, Turret.BoxSize / 2f, _direction, out var hitInfo,
                    _rotation, Turret.Range, Turret.OppositeUnitLayer))
            {
                OnSeeingEnemy?.Invoke(GetLookDir(MovableGfxPrefab.transform, hitInfo.transform));
                
               
                
                if (!_isAttacking)
                {
                    Attack();
                }
            }
            else
            {
                ResetAttackStateIfNeeded();
            }
        }

        private void Attack()
        {
            _isAttacking = true;
            _attackCoroutine = CoroutineManager.Instance.StartManagedCoroutine(AttackLoop(Turret.InitialAttackDelay));
        }

        private IEnumerator AttackLoop(float initialAttackDelay)
        {
            yield return new WaitForSeconds(initialAttackDelay);
            OnAttack?.Invoke();
            _isAttacking = false;
        }

        private void ResetAttackStateIfNeeded()
        {
            if (_attackCoroutine != null)
            {
                _attackCoroutine.Stop();
                _attackCoroutine = null;
            }
            OnSeeingNothing?.Invoke();
            _isAttacking = false;
        }
        
        private Quaternion GetLookDir(Transform turret, Transform target)
        {
            var rot = Quaternion.LookRotation(target.position - turret.position) * Quaternion.Euler(0, -90, 0);
            return rot;
        }
        

        #region GameLifecycle

        protected override void HandlePause()
        {
            _attackCoroutine?.Pause();
        }

        protected override void HandleResume()
        {
            _attackCoroutine?.Resume();
        }

        protected override void HandleGameEnd()
        {
            ResetAttackStateIfNeeded();
        }

        protected override void HandleGameReset()
        {
            ResetAttackStateIfNeeded();
        }

        #endregion
    }
}