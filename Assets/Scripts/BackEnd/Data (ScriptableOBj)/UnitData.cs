using System.Collections.Generic;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.units;
using BackEnd.Base_Classes;
using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Data__ScriptableOBj_
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Unit", order = 1)]
    public class UnitData : DataObject<UnitType>
    {

        [Tooltip("Delay in seconds between button press and unit deployment.")]
        [Min(0f)]
        public float DeployDelayTime;

        [FormerlySerializedAs("_moneyWhenKilled")]
        [Header("Enemy Unit Reward")]
        [Tooltip("Amount of money rewarded when this enemy unit is destroyed.")]
        [Min(0)]
        public int MoneyWhenKilled;

        [Header("Tag & Layer Configuration")]
        [Tooltip("Tag of the opposing base the unit will move toward.")]
        [SerializeField, TagSelector] public string OppositeBaseTag;

        [Tooltip("Tag used to identify enemy units this unit will attack.")]
        [SerializeField, TagSelector] public string OppositeUnitTag;

        [Tooltip("Layer used to detect enemy Units & Bases during targeting.")]
        [SerializeField] public LayerMask OppositeUnitMask;

        [Tooltip("Tag used to identify friendly units (used to avoid collision or combat).")]
        [SerializeField, TagSelector] public string FriendlyUnitTag;



        [Header("Combat & Behavior Parameters")]

        [Tooltip("Movement speed of the unit.")]
        [Min(0f)]
        public float Speed = 1;

        [Tooltip("Maximum health of the unit.")]
        [Min(1)]
        public int Health = 1;

        [Tooltip("Damage dealt with each attack.")]
        [Min(0)]
        public int Strength = 1;

        [Tooltip("Time delay before the unit performs its first attack (lower is faster).")]
        [Min(0f)]
        public float InitialAttackDelay = 1;

        [Header("Detetaction Parameters")]
        [Tooltip("Raycast distance used to detect nearby friendly units.")]
        [Min(0f)]
        public float RayLengthForFriendlyUnit = 2;

        [Tooltip("How far the unit can detect enemy targets.")]
        [Min(0)]
        public int Range;

        [Header("Debug Visualization")]
        [Tooltip("Size of the gizmo box used to visualize detection range.")]
        public Vector3 boxSize = new Vector3(0.5f, 0.5f, 0.5f);

        [Tooltip("Color of the gizmo box drawn for debugging purposes.")]
        public Color boxColor = Color.red;



        public override void UpgradeAge(List<LevelUpDataBase> upgradeDataList)
        {
            for (int i = 0; i < upgradeDataList.Count; i++)
            {
                if (upgradeDataList[i] is UnitLevelUpData levelUpData && Type == levelUpData.Type)
                {
                    //---Core Stat---
                    Range += levelUpData._range;
                    Speed += levelUpData._speed;
                    Health += levelUpData._health;
                    Strength += levelUpData._strength;
                    InitialAttackDelay *= 1f - (levelUpData.AttackDelayReductionPercent / 100f);
                    InitialAttackDelay = Mathf.Max(0.1f, InitialAttackDelay);

                    //---Prefab---
                    Prefab = levelUpData.Prefab;

                    //---AgeStage---
                    AgeStage = levelUpData.AgeStage;
                    break;
                }
            } 

        }
    }
}