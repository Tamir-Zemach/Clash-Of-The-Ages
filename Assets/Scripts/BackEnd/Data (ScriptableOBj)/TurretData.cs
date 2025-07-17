using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.turrets
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "TurretData", order = 2)]
    public class TurretData : ScriptableObject, IUpgradable<TurretType>
    {
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
            else
            {
                EnemyAgeManager.Instance.OnAgeUpgrade += UpgradeAge;
            }
        }
        public void UpgradeAge(List<LevelUpDataBase> upgradeDataList)
        {
            for (int i = 0; i < upgradeDataList.Count; i++)
            {
                if (upgradeDataList[i] is TurretLevelUpData levelUpData && Type == levelUpData.Type)
                {
                    //---Core Stat---
                    Range += levelUpData.Range;
                    BulletSpeed += levelUpData.BulletSpeed;
                    BulletStrength += levelUpData.BulletStrength;
                    InitialAttackDelay *= 1f - (levelUpData.AttackDelayReductionPercent / 100f);
                    InitialAttackDelay = Mathf.Max(0.1f, InitialAttackDelay);

                    //---Prefab---
                    Prefab = levelUpData.Prefab;

                    //---AgeStage---
                    AgeStage = levelUpData.AgeStage;
                }
            }
        }

        [Header("Turret Identity")]
        [Tooltip("The age stage during which this turret becomes available.")]
        [field: SerializeField] public AgeStageType AgeStage { get; private set; }

        [Tooltip("Type of turret represented by this configuration.")]
        [field: SerializeField] public TurretType Type { get; private set; }

        [Tooltip("Indicates whether this turret is controlled by the friendly faction.")]
        [field: SerializeField] public bool IsFriendly { get; private set; }

        [Header("Prefab Settings")]
        [Tooltip("Prefab to instantiate when this turret is deployed.")]
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [Header("Detection Settings")]
        [Tooltip("Layer that contains enemy units to detect.")]
        [field: SerializeField] public LayerMask OppositeUnitLayer { get; private set; }

        [Tooltip("Tag assigned to enemy units.")]
        [field: SerializeField, TagSelector] public string OppositeUnitTag { get; private set; }
        [Tooltip("Tag assigned to the opposite base.")]

        [field: SerializeField, TagSelector] public string OppositeBase { get; private set; }

        [Tooltip("Tag assigned to the friendly base.")]
        [field: SerializeField, TagSelector] public string FriendlyBase { get; private set; }


        [Tooltip("Maximum detection range of the turret.")]
        [Min(0f)]
        [field: SerializeField] public float Range { get; private set; }

        [Tooltip("Size of the BoxCast used for target detection.")]
        [field: SerializeField] public Vector3 BoxSize = Vector3.one;

        [Header("Attack Settings")]

        [Min(0f)]
        [Tooltip("The amount of time before a Unit Attacks (when lower its faster)")]
        [field: SerializeField] public float InitialAttackDelay { get; private set; }

        [Tooltip("Base damage each bullet inflicts.")]
        [Min(0)]
        [field: SerializeField] public int BulletStrength { get; private set; }

        [Tooltip("Speed applied to bullets when fired.")]
        [Min(0)]
        [field: SerializeField] public int BulletSpeed { get; private set; }

    }
}