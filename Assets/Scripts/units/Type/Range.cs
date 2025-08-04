using BackEnd.Data__ScriptableOBj_;
using units.Behavior;
using UnityEngine;

namespace units.Type
{
    [RequireComponent(typeof(UnitBaseBehaviour))]
    public class Range : MonoBehaviour
    {
        private UnitBaseBehaviour _unitBaseBehaviour;
        private UnitData _unit;
        private int _strength;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;

        private void Awake()
        {
            _unitBaseBehaviour = GetComponent<UnitBaseBehaviour>();
            if (_unitBaseBehaviour != null)
            {
                _unitBaseBehaviour.OnAttack += Attack;
            }
        }
        private void Start()
        {
            _unit = _unitBaseBehaviour.Unit;
        }


        public void FireProjectile(GameObject target, int strength)
        {
            if (target == null) return;

            var instance = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            var bulletScript = instance.GetComponent<RangeBullet>();

            if (bulletScript != null)
            {
                bulletScript.Initialize(target.transform, strength);
            }

            target = null; // clear after shot
        }

        private void Attack(GameObject target, int strength)
        {
            if (target == null) return;
            _strength =  strength;
            var instance = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            if (instance == null) return;
            var bulletScript = instance.GetComponent<RangeBullet>();
            if (bulletScript == null) return;
            bulletScript.Initialize(target.transform, _strength);

        }

    }
}
