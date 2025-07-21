using System;
using System.Collections;
using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data__ScriptableOBj_;
using UnityEngine;
using UnityEngine.Serialization;

namespace turrets
{
    public class TurretBaseBehavior : MonoBehaviour
    {

        public event Action OnAttack;

        //"Origin point from which the BoxCast is performed
        private Transform _detectionOrigin;

        [Tooltip("Transform from where bullets will be instantiated.")]
        [SerializeField] private Transform _bulletSpawnPos;

        [FormerlySerializedAs("_bulletPrefab")] [Tooltip("Projectile prefab to spawn when attacking.")]
        public GameObject BulletPrefab;

        private Vector3 _origin;
        private Vector3 _direction;
        private Quaternion _rotation;
        private bool _isAttacking;
        private Coroutine _currentCoroutine;
        public TurretData Turret { get; private set; }

        public Vector3 Origin => _origin;
        public Vector3 Direction => _direction;
        public Quaternion Rotation => _rotation;

        public void Initialize(TurretData turretData)
        {
            Turret = turretData;

            var baseObject = GameObject.FindGameObjectWithTag(Turret.FriendlyBase);
            var enemyBaseObject = GameObject.FindGameObjectWithTag(Turret.OppositeBase);
            if (baseObject != null)
            {
                _detectionOrigin = baseObject.transform;
                _origin = _detectionOrigin.position;
                _direction = (enemyBaseObject.transform.position - transform.position).normalized;
                _rotation = _detectionOrigin.rotation;
            }
            else
            {
                Debug.LogWarning("TurretBaseBehavior: Could not find object with tag " + Turret.FriendlyBase);
            }
        }


        private void Update()
        {
            CheckForEnemies();
        }

        private void CheckForEnemies()
        {
            if (Physics.BoxCast(_origin, Turret.BoxSize, _direction, out var hitInfo,
                    _detectionOrigin.rotation, Turret.Range, Turret.OppositeUnitLayer))
            {
                gameObject.transform.LookAt(hitInfo.transform);
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
            _currentCoroutine = StartCoroutine(AttackLoop(Turret.InitialAttackDelay));
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
            if (_currentCoroutine == null) return;
            StopAllCoroutines();
            _currentCoroutine = null;
            _isAttacking = false;
        }
    }
}