using Assets.Scripts.Backend.Data;
using Assets.Scripts.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using Assets.Scripts.turrets;
using Assets.Scripts.Ui.TurretButton;
using Assets.Scripts.units;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the age-based progression system for both player and enemy factions,
/// applying unit stat upgrades, prefab changes, and UI visual updates.
/// </summary>
public class AgeUpgrade : PersistentMonoBehaviour<AgeUpgrade>
{
    #region Fields & Properties

    public int CurrentPlayerAge { get; private set; } = 1;
    public int CurrentEnemyAge { get; private set; } = 1;


    private UnitDeployButton[] _unitDeployButtons;
    private UnitUpgradeButton[] _unitUpgradeButtons;
    private SpecialAttackButton _specialAttackButton;
    private TurretButton[] _turretButtons;


    #endregion

    #region Core Upgrade Logic




    /// <summary>
    /// Loops through all unit data and applies matching upgrades based on type, age, and faction.
    /// For friendly units, it also updates the UI and registered sprites/prefabs.
    /// Enemy units receive stat boosts and updated reward values.
    /// </summary>
    public void ApplyUpgradesToUnits(List<UnitData> unitDataList, List<UnitLevelUpData> unitLevelUpDataList, int currentAge, bool isFriendly)
    {
        if (NullChecks.DataListsNullCheck(unitDataList, unitLevelUpDataList)) return;

        for (int i = 0; i < unitDataList.Count; i++)
        {
            foreach (UnitLevelUpData levelUpData in unitLevelUpDataList)
            {
                if (IsUpgradeRelevant<UnitData, UnitLevelUpData, UnitType>(unitDataList[i],
                    levelUpData,
                    currentAge,
                    isFriendly))
                {
                    UpgradeCoreStats(unitDataList[i], levelUpData);
                    UpdateDataType<UnitData, UnitLevelUpData, UnitType>(unitDataList[i], levelUpData);
                    UpdateDataPrefab<UnitData, UnitLevelUpData, UnitType>(unitDataList[i], levelUpData);

                    if (!isFriendly)
                    {
                        UpdateUnitReward(unitDataList[i], levelUpData);
                    }
                    break;
                }
            }
        }
    }
    public void ApplyUpgradeToTurrets(List<TurretData> turretDataList,
                  List<TurretLevelUpData> turretLevelUpData,
                  int currentAge,
                  bool isFriendly)
    {
        if (NullChecks.DualDataNullCheck(turretDataList, turretLevelUpData)) return;

        for (int i = 0; i < turretDataList.Count; i++)
        {
            foreach (TurretLevelUpData levelUpData in turretLevelUpData)
            {

                if (IsUpgradeRelevant<TurretData, TurretLevelUpData, TurretType>(turretDataList[i], levelUpData, currentAge, isFriendly))
                {
                    UpdateDataType<TurretData, TurretLevelUpData, TurretType>(turretDataList[i], levelUpData);
                    UpdateDataPrefab<TurretData, TurretLevelUpData, TurretType>(turretDataList[i], levelUpData);
                    UpgradeTurretCoreStats(turretDataList[i], levelUpData);
                }
            }
        }

    }



    /// <summary>
    /// Applies an upgrade to a special attack if it matches the faction and current age level.
    /// Updates both the prefab and, if friendly, the UI button’s sprite to reflect the new upgrade.
    /// </summary>
    public void ApplySpecialAttackUpgrade(SpecialAttackData specialAttackData,
                      SpecialAttackLevelUpData specialAttackLevelUpData,
                      int currentAge,
                      bool isFriendly)
    {
        if (NullChecks.DualDataNullCheck(specialAttackData, specialAttackLevelUpData)) return;

        UpdateDataType<SpecialAttackData, SpecialAttackLevelUpData, AgeStageType>(specialAttackData, specialAttackLevelUpData);
        if (IsUpgradeRelevant<SpecialAttackData, SpecialAttackLevelUpData, AgeStageType>(specialAttackData, specialAttackLevelUpData, currentAge, isFriendly))
        {
            UpdateDataPrefab<SpecialAttackData, SpecialAttackLevelUpData, AgeStageType>(specialAttackData, specialAttackLevelUpData);
        }
    }






    /// <summary>
    /// Determines whether a level-up upgrade is valid for a specific data object (like a unit or special attack),
    /// based on matching type, faction alignment, and age stage requirements.
    /// </summary>
    /// <typeparam name="TData">The current data class (e.g. UnitData, SpecialAttackData) implementing IUpgradable&lt;TType&gt;.</typeparam>
    /// <typeparam name="TLevelUpData">The upgrade data class (e.g. UnitLevelUpData, SpecialAttackLevelUpData) implementing ILevelUpData&lt;TType&gt;.</typeparam>
    /// <typeparam name="TType">The shared type used to identify the object (e.g. UnitType, SpecialAttackType).</typeparam>
    /// <param name="data">The current data instance that may receive an upgrade.</param>
    /// <param name="levelUpData">The level-up data containing upgrade values and criteria.</param>
    /// <param name="currentAge">The age level currently reached in the game, used to determine if the upgrade is unlocked.</param>
    /// <param name="isFriendly">Indicates whether the upgrade applies to the friendly side or enemy side.</param>
    /// <returns>True if the upgrade matches the object's type, side (friendly/enemy), and current age stage; otherwise, false.</returns>
    private bool IsUpgradeRelevant<TData, TLevelUpData, TType>(
        TData data,
        TLevelUpData levelUpData,
        int currentAge,
        bool isFriendly)
        where TData : IUpgradable<TType>
        where TLevelUpData : IUpgradable<TType>
    {
        return data.Type.Equals(levelUpData.Type) &&
               data.IsFriendly == isFriendly &&
               levelUpData.IsFriendly == isFriendly &&
               levelUpData.AgeStage == currentAge;
    }

    public void UpdateDataPrefab<TData, TLevelUpData, TType>(
                TData data,
                TLevelUpData levelUpData)
                where TData : IUpgradable<TType>
                where TLevelUpData : IUpgradable<TType>
    {
        data.SetPrefab(levelUpData.Prefab);
    }
    public void UpdateDataType<TData, TLevelUpData, TType>(
                TData data,
                TLevelUpData levelUpData)
                where TData : IUpgradable<TType>
                where TLevelUpData : IUpgradable<TType>
    {
        data.SetType(levelUpData.Type);
    }


    public void UpgradeCoreStats(UnitData unit, UnitLevelUpData levelUpData)
    {
        unit.Range += levelUpData._range;
        unit.Speed += levelUpData._speed;
        unit.Health += levelUpData._health;
        unit.Strength += levelUpData._strength;

        // Use percent-based reduction for attack delay
        unit.InitialAttackDelay *= 1f - (levelUpData.AttackDelayReductionPercent / 100f);
        unit.InitialAttackDelay = Mathf.Max(0.1f, unit.InitialAttackDelay);
    }
    public void UpgradeTurretCoreStats(TurretData turretData, TurretLevelUpData levelUpData)
    {
        turretData.Range += levelUpData.Range;
        turretData.BulletSpeed += levelUpData.BulletSpeed;
        turretData.BulletStrength += levelUpData.BulletStrength;

        // Use percent-based reduction for attack delay
        turretData.InitialAttackDelay *= 1f - (levelUpData.AttackDelayReductionPercent / 100f);
        Mathf.Max(0.1f, turretData.InitialAttackDelay);
    }


    public void UpdateUnitReward(UnitData unit, UnitLevelUpData levelUpData)
    {
        unit._moneyWhenKilled += levelUpData._moneyWhenKilled;

    }

    public void AdvanceAge(bool isFriendly)
    {
        if (isFriendly) CurrentPlayerAge++;
        else CurrentEnemyAge++;
    }



    #endregion


}

