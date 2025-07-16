using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using Assets.Scripts.units;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit", order = 1)]
public class UnitData : ScriptableObject, IUpgradable<UnitType>
{
    [Tooltip("The age stage during which this unit becomes available.")]
    [field: SerializeField] public AgeStageType AgeStage { get; private set; }


    [Header("Unit Identity")]
    [Tooltip("Specifies the type of unit.")]
    [field: SerializeField] public UnitType Type { get; private set; }

    [Tooltip("Indicates whether this unit belongs to the friendly faction.")]
    [field: SerializeField] public bool IsFriendly { get; private set; }



    [Header("Deployment Settings")]
    [Tooltip("Prefab to instantiate when deploying this unit.")]
    [field: SerializeField] public GameObject Prefab { get; private set; }

    [Tooltip("Cost required to deploy this unit.")]
    [Min(0)]
    public int Cost;

    [Tooltip("Delay in seconds between button press and unit deployment.")]
    [Min(0f)]
    public float DeployDelayTime;

    [Header("Enemy Unit Reward")]
    [Tooltip("Amount of money rewarded when this enemy unit is destroyed.")]
    [Min(0)]
    public int _moneyWhenKilled;

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