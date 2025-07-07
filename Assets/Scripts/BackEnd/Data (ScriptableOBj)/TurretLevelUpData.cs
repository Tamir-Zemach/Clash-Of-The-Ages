using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "TurretLevelUpData", menuName = "TurretLevelUpData", order = 6)]
    public class TurretLevelUpData : ScriptableObject, IUpgradable<TurretType>
    {
        [Header("Turret Identity")]
        [Tooltip("Defines the turret type for this level-up configuration.")]
        [SerializeField] private TurretType _turretType;

        [Tooltip("Determines if this turret belongs to the friendly faction.")]
        [SerializeField] private bool _isFriendly;

        [Tooltip("Specifies the age stage this upgrade is applied in.")]
        [SerializeField] private AgeStageType _ageStage;

        [Header("Prefab Reference")]
        [Tooltip("Prefab to instantiate after the turret is upgraded.")]
        [SerializeField] private GameObject _turretPrefab;

        [Header("Detection Settings")]
        [Tooltip("Additional detection range to apply on top of the turret's base range.")]
        [Min(0f)]
        public float Range;

        [Header("Attack Settings")]
        [Tooltip("Percentage reduction applied to initial attack delay after upgrade.\n" +
                 "For example, a value of 20 reduces the delay by 20%.")]
        [Min(0f)]
        public float AttackDelayReductionPercent;

        [Tooltip("Bonus damage added to the turret's original bullet strength.")]
        [Min(0)]
        public int BulletStrength;

        [Tooltip("Additional bullet speed applied to the base turret projectile.")]
        [Min(0)]
        public int BulletSpeed;

        // Public accessors
        public TurretType Type => _turretType;
        public bool IsFriendly => _isFriendly;
        public int AgeStage => (int)_ageStage;
        public GameObject Prefab => _turretPrefab;

        public void SetPrefab(GameObject prefab)
        {
            _turretPrefab = prefab;
        }
        public void SetType(TurretType turretType)
        {
            _turretType = turretType;
        }
    }
}