using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "TurretLevelUpData", menuName = "TurretLevelUpData", order = 6)]
    public class TurretLevelUpData : LevelUpDataBase
    {
        [Header("Turret Identity")]
        [Tooltip("Defines the turret type for this level-up configuration.")]
        [field: SerializeField] public TurretType Type { get; private set; }

        [Tooltip("Indicates whether this unit belongs to the friendly faction.")]
        [field: SerializeField] public bool IsFriendly { get; private set; }

        [Header("Deployment Settings")]
        [Tooltip("Prefab to instantiate when deploying this unit.")]
        [field: SerializeField] public GameObject Prefab { get; private set; }
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



 
    }
}