using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.Data;
using BackEnd.Base_Classes;
using UnityEngine;

namespace BackEnd.Data__ScriptableOBj_
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "TurretData", order = 2)]
    public class TurretData : DataObject<TurretType>
    {

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
        
        [field: SerializeField] public int MinBulletStrength { get; private set; }
        
        [field: SerializeField] public int MaxBulletStrength { get; private set; }
        

        [Tooltip("Speed applied to bullets when fired.")]
        [Min(0)]
        [field: SerializeField] public int BulletSpeed { get; private set; }

        public override void UpgradeAge(List<LevelUpDataBase> upgradeDataList)
        {
            for (int i = 0; i < upgradeDataList.Count; i++)
            {
                if (upgradeDataList[i] is TurretLevelUpData levelUpData && Type == levelUpData.Type)
                {
                    //---Core Stat---
                    Range += levelUpData.Range;
                    BulletSpeed += levelUpData.BulletSpeed;
                    
                    MinBulletStrength += levelUpData.MinBulletStrength;
                    MaxBulletStrength += levelUpData.MaxBulletStrength;
                    
                    InitialAttackDelay *= 1f - (levelUpData.AttackDelayReductionPercent / 100f);
                    InitialAttackDelay = Mathf.Max(0.1f, InitialAttackDelay);

                    //---Prefab---
                    Prefab = levelUpData.Prefab;

                    //---AgeStage---
                    AgeStage = levelUpData.AgeStage;
                }
            }
        }


    }
}