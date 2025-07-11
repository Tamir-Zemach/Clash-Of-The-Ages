using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.turrets
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "TurretData", order = 2)]
    public class TurretData : ScriptableObject, IUpgradable<TurretType>
    {
        [Header("Turret Identity")]
        [Tooltip("The age stage during which this turret becomes available.")]
        [SerializeField] private AgeStageType _stageType;

        [Tooltip("Type of turret represented by this configuration.")]
        [SerializeField] private TurretType _turretType;

        [Tooltip("Indicates whether this turret is controlled by the friendly faction.")]
        [SerializeField] private bool _isFriendly;

        [Header("Prefab Settings")]
        [Tooltip("Prefab to instantiate when this turret is deployed.")]
        [SerializeField] private GameObject _unitPrefab;

        [Header("Detection Settings")]
        [Tooltip("Layer that contains enemy units to detect.")]
        public LayerMask OppositeUnitLayer;

        [Tooltip("Tag assigned to enemy units.")]
        [TagSelector] public string OppositeUnitTag;

        [Tooltip("Tag assigned to the friendly base.")]
        [TagSelector] public string FriendlyBase;

        [Tooltip("Maximum detection range of the turret.")]
        [Min(0f)]
        public float Range;

        [Tooltip("Size of the BoxCast used for target detection.")]
        public Vector3 BoxSize = Vector3.one;

        [Header("Attack Settings")]

        [Min(0f)]
        [Tooltip("The amount of time before a Unit Attacks (when lower its faster)")]
        public float InitialAttackDelay;

        [Tooltip("Base damage each bullet inflicts.")]
        [Min(0)]
        public int BulletStrength;

        [Tooltip("Speed applied to bullets when fired.")]
        [Min(0)]
        public int BulletSpeed;

        // Public properties
        public TurretType Type => _turretType;
        public bool IsFriendly => _isFriendly;
        public int AgeStage => (int)_stageType;
        public GameObject Prefab => _unitPrefab;

        // Setter to support upgrade replacement
        public void SetPrefab(GameObject prefab)
        {
            _unitPrefab = prefab;
        }
        public void SetType(TurretType turretType)
        {
            _turretType = turretType;
        }
    }
}