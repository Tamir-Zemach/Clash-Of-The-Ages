using UnityEngine;

namespace turrets
{
    public class TurretBulletSpawner : MonoBehaviour
    {
        [Tooltip("Transform from where bullets will be instantiated.")]
        [SerializeField] private Transform _bulletSpawnPos;
        
        [Tooltip("Projectile prefab to spawn when attacking.")]
        public GameObject BulletPrefab;

        private TurretBaseBehavior _turretBaseBehavior;
        private Quaternion _bulletSpawnRot;

        private void Awake()
        {
            _turretBaseBehavior = GetComponentInParent<TurretBaseBehavior>();
            _turretBaseBehavior.OnSeeingEnemy += SetRotation;
        }

        private void SetRotation(Quaternion rotation)
        {
            //TODO: Fix rot
            _bulletSpawnRot =  rotation;
        }

        public void SpawnBullet()
        {
            Instantiate(BulletPrefab, _bulletSpawnPos.position, _bulletSpawnRot);
        }
        
    }
}