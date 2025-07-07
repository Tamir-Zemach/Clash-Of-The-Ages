using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpecialAttackData", menuName = "SpecialAttackData", order = 4)]
    public class SpecialAttackData : ScriptableObject, IUpgradable<AgeStageType>
    {
        [Header("Identification")]
        [Tooltip("Specifies what age stage this upgrade belongs to.")]
        [SerializeField] private AgeStageType _ageStage;

        [Tooltip("Indicates if this special attack belongs to the friendly faction.")]
        [SerializeField] private bool _isFriendly;

        [Header("Deployment Settings")]
        [Tooltip("The prefab to instantiate when deployed.")]
        [SerializeField] private GameObject _specialAttackPrefab;
        [Tooltip("Cost to deploy or use this special attack.")]
        [Min(0)]
        [SerializeField] private int cost;

        // Public getters
        public AgeStageType Type => _ageStage;
        public bool IsFriendly => _isFriendly;
        public GameObject Prefab => _specialAttackPrefab;
        public int AgeStage => (int)_ageStage;
        public int Cost => cost;

        // Upgradability hook
        public void SetPrefab(GameObject prefab)
        {
            _specialAttackPrefab = prefab;
        }

        public void SetType(AgeStageType type)
        {
            _ageStage = type;
        }
    }
}