using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using UnityEngine;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradeDataStorage : OneInstanceClass<UpgradeDataStorage>
    {
        // Unit stat upgrades: UnitType → StatType → count
        private readonly Dictionary<UnitType, Dictionary<StatType, int>> _unitStatUpgrades = new();

        // Global upgrades: UpgradeType → count
        private readonly Dictionary<UpgradeType, int> _globalUpgrades = new();

        // Age upgrades: UnitType → AgeStageType
        private readonly Dictionary<UnitType, AgeStageType> _ageUpgrades = new();

        // Public accessors
        public IReadOnlyDictionary<UnitType, Dictionary<StatType, int>> UnitStatUpgrades => _unitStatUpgrades;
        public IReadOnlyDictionary<UpgradeType, int> GlobalUpgrades => _globalUpgrades;
        public IReadOnlyDictionary<UnitType, AgeStageType> AgeUpgrades => _ageUpgrades;

        /// <summary>
        /// Records a stat upgrade applied to a specific unit.
        /// </summary>
        public void RegisterUnitStatUpgrade(UnitType unitType, StatType statType)
        {
            if (!_unitStatUpgrades.ContainsKey(unitType))
            {
                _unitStatUpgrades[unitType] = new Dictionary<StatType, int>();
            }
            if (_unitStatUpgrades[unitType].ContainsKey(statType))
            {
                _unitStatUpgrades[unitType][statType]++;
            }
            else
            {
                _unitStatUpgrades[unitType][statType] = 1; 
            }

        }

        /// <summary>
        /// Records a global upgrade applied.
        /// </summary>
        public void RegisterGlobalUpgrade(UpgradeType upgradeType)
        {
            if (_globalUpgrades.ContainsKey(upgradeType))
            {
                _globalUpgrades[upgradeType]++;
            }
            else
            {
                _globalUpgrades[upgradeType] = 1;
            }
                
        }

        /// <summary>
        /// Records an age upgrade applied to a unit.
        /// </summary>
        public void RegisterAgeUpgrade(UnitType unitType, AgeStageType ageStage)
        {
            _ageUpgrades[unitType] = ageStage;
        }

        // Optional: Query helpers
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
    }
}