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
        public event Action OnAttack;

        [Tooltip("Transform from where bullets will be instantiated.")]
        [SerializeField] private Transform _bulletSpawnPos;

        [FormerlySerializedAs("_bulletPrefab")]
        [Tooltip("Projectile prefab to spawn when attacking.")]
        public GameObject BulletPrefab;

        private Transform _detectionOrigin;
        private Vector3 _origin;
        private Vector3 _direction;
        private Quaternion _rotation;
        private bool _isAttacking;
        private ManagedCoroutine _attackCoroutine;

        public TurretData Turret { get; private set; }

        public Vector3 Origin => _origin;
        public Vector3 Direction => _direction;
        public Quaternion Rotation => _rotation;

        public void Initialize(TurretData turretData)
        {
            Turret = turretData;

            var baseObject = GameObject.FindGameObjectWithTag(Turret.FriendlyBase);
            var enemyBaseObject = GameObject.FindGameObjectWithTag(Turret.OppositeBase);

            if (baseObject != null && enemyBaseObject != null)
            {
                _detectionOrigin = baseObject.transform;
                _origin = _detectionOrigin.position;
                _direction = (enemyBaseObject.transform.position - transform.position).normalized;
                _rotation = _detectionOrigin.rotation;
            }
            else
            {
                Debug.LogWarning($"TurretBaseBehavior: Could not find base objects with tags {Turret.FriendlyBase} or {Turret.OppositeBase}");
            }
        }

        private void Update()
        {
            if (!GameStates.Instance.GameIsPlaying) return;
            CheckForEnemies();
        }

        private void CheckForEnemies()
        {
            if (Physics.BoxCast(_origin, Turret.BoxSize, _direction, out var hitInfo,
                    _detectionOrigin.rotation, Turret.Range, Turret.OppositeUnitLayer))
            {
                transform.LookAt(hitInfo.transform);
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
            Instantiate(BulletPrefab, _bulletSpawnPos.position, _bulletSpawnPos.rotation);
            _isAttacking = false;
        }

        private void ResetAttackStateIfNeeded()
        {
            if (_attackCoroutine != null)
            {
                _attackCoroutine.Stop();
                _attackCoroutine = null;
            }

            _isAttacking = false;
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