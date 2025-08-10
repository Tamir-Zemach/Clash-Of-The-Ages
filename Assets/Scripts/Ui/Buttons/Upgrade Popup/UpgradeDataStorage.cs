using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using Configuration;
using Managers;
using UnityEngine;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradeDataStorage : OneInstanceClass<UpgradeDataStorage>
    {
        public event Action<UnitType, StatType> OnUnitUpgradeRegistered;
        public event Action<UpgradeType> OnGlobalUpgradeRegistered;
        public event Action<UnitType, AgeStageType> OnAgeUnitUpgradeRegistered;

        private readonly Dictionary<UnitType, Dictionary<StatType, int>> _unitStatUpgrades = new();
        private readonly Dictionary<UpgradeType, int> _globalUpgrades = new();
        private readonly Dictionary<UnitType, AgeStageType> _ageUpgrades = new();
        public UpgradeDataStorage()
        {
            GameStates.Instance.OnGameReset += ResetAllUpgradeData;
        }
        
        public void RegisterUnitStatUpgrade(UnitType unitType, StatType statType)
        {
            if (!_unitStatUpgrades.ContainsKey(unitType))
                _unitStatUpgrades[unitType] = new Dictionary<StatType, int>();

            if (_unitStatUpgrades[unitType].ContainsKey(statType))
                _unitStatUpgrades[unitType][statType]++;
            else
                _unitStatUpgrades[unitType][statType] = 1;
            
            OnUnitUpgradeRegistered?.Invoke(unitType, statType);
        }

        public void RegisterGlobalUpgrade(UpgradeType upgradeType)
        {
            if (_globalUpgrades.ContainsKey(upgradeType))
                _globalUpgrades[upgradeType]++;
            else
                _globalUpgrades[upgradeType] = 1;

            OnGlobalUpgradeRegistered?.Invoke(upgradeType);
        }

        public void RegisterAgeUpgrade(UnitType unitType, AgeStageType ageStage)
        {
            _ageUpgrades[unitType] = ageStage;
            
            OnAgeUnitUpgradeRegistered?.Invoke(unitType, ageStage);
        }

        public int GetUnitStatUpgradeCount(UnitType unitType, StatType statType)
        {
            return _unitStatUpgrades.TryGetValue(unitType, out var stats) &&
                   stats.TryGetValue(statType, out var count)
                ? count
                : 0;
        }

        public int GetGlobalUpgradeCount(UpgradeType upgradeType)
        {
            return _globalUpgrades.TryGetValue(upgradeType, out var count) ? count : 0;
        }

        public bool HasAgeUpgrade(UnitType unitType)
        {
            return _ageUpgrades.ContainsKey(unitType);
        }
        
        private void ResetAllUpgradeData()
        {
            _unitStatUpgrades.Clear();
            _globalUpgrades.Clear();
            _ageUpgrades.Clear();
        }
        
    }
}