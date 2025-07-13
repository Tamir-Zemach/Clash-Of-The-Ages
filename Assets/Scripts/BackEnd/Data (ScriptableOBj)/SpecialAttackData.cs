using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using Assets.Scripts.units;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "SpecialAttackData", menuName = "SpecialAttackData", order = 4)]
    public class SpecialAttackData : ScriptableObject, IUpgradable<SpecialAttackType>
    {
        [Tooltip("The age stage during which this unit becomes available.")]
        [field: SerializeField] public AgeStageType AgeStage { get; private set; }

        [Header("Unit Identity")]
        [Tooltip("Specifies the type of unit.")]
        [field: SerializeField] public SpecialAttackType Type { get; private set; }

        [Tooltip("Indicates whether this unit belongs to the friendly faction.")]
        [field: SerializeField] public bool IsFriendly { get; private set; }


        [Header("Deployment Settings")]
        [Tooltip("Prefab to instantiate when deploying this unit.")]
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [Tooltip("Cost to deploy or use this special attack.")]
        [Min(0)]
        [field: SerializeField, TagSelector] public string SpawnPosTag { get; private set; }

        private void Awake()
        {
            if (GameDataRepository.Instance != null)
            {
                GameDataRepository.Instance.OnInitialized += Inisilize;
            }
        }
        private void Inisilize()
        {
            if (IsFriendly)
            {
                GameManager.Instance.OnAgeUpgrade += UpgradeAge;
            }
        }
        public void UpgradeAge(List<LevelUpDataBase> upgradeDataList)
        {
            for (int i = 0; i < upgradeDataList.Count; i++)
            {
                if (upgradeDataList[i] is SpecialAttackLevelUpData levelUpData && Type == levelUpData.Type)
                {
                    AgeStage = levelUpData.AgeStage;
                    Prefab = levelUpData.Prefab;
                }
            }
        }
    }
}