using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpecialAttackLevelUpData", menuName = "SpecialAttackLevelUpData", order = 5)]
    public class SpecialAttackLevelUpData : ScriptableObject, IUpgradable<AgeStageType>
    {
        [Header("Identification")]
        [Tooltip("Indicates whether this upgrade is for a friendly faction.")]
        [SerializeField] private bool _isFriendly;

        [Header("Upgrade Configuration")]
        [Tooltip("Specifies the age stage this upgrade belongs to.")]
        [SerializeField] private AgeStageType _ageStage;

        [Tooltip("The prefab to instantiate after upgrade is applied.")]
        [SerializeField] private GameObject _specialPrefab;

        // Public getters
        public AgeStageType Type => _ageStage;
        public bool IsFriendly => _isFriendly;
        public int AgeStage => (int)_ageStage;
        public GameObject Prefab => _specialPrefab;

        // Prefab setter to support upgrade logic
        public void SetPrefab(GameObject prefab)
        {
            _specialPrefab = prefab;
        }
        public void SetType(AgeStageType type)
        {
            _ageStage = type;
        }
    }
}