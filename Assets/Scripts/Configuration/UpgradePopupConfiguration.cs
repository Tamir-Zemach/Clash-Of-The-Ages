using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Configuration
{
    public class UpgradePopupConfiguration : OneInstanceMonoBehaviour<UpgradePopupConfiguration>
    {
        [Header("Shared Spawn Chances")]
        [Range(0f, 1f)] [SerializeField] private float _unitSpawnChance = 1f;
        [Range(0f, 1f)] [SerializeField] private float _globalSpawnChance = 1f;
        [Range(0f, 1f)] [SerializeField] private float _ageUpgradeSpawnChance = 1f;

        [Header("Slot Prefabs")]
        [SerializeField] private int _maxInstancesPerUnitPrefab = 3;
        [SerializeField] private List<GameObject> _unitPrefabs;

        [SerializeField] private int _maxInstancesPerGlobalPrefab = 2;
        [SerializeField] private List<GameObject> _globalPrefabs;

        [SerializeField] private List<GameObject> _ageUpgradePrefabs;

        private readonly Dictionary<GameObject, int> _instantiationCounts = new();

        // Public accessors for debugger or other systems
        public List<GameObject> UnitPrefabs => _unitPrefabs;
        public List<GameObject> GlobalPrefabs => _globalPrefabs;
        public List<GameObject> AgeUpgradePrefabs => _ageUpgradePrefabs;
        public IReadOnlyDictionary<GameObject, int> InstantiationCounts => _instantiationCounts;

        public List<GameObject> GetEligiblePrefabs()
        {
            var eligible = new List<GameObject>();

            eligible.AddRange(_unitPrefabs.Where(p =>
                PassedChance(_unitSpawnChance) &&
                GetRemainingQuota(p, _maxInstancesPerUnitPrefab) > 0));

            eligible.AddRange(_globalPrefabs.Where(p =>
                PassedChance(_globalSpawnChance) &&
                GetRemainingQuota(p, _maxInstancesPerGlobalPrefab) > 0));

            eligible.AddRange(_ageUpgradePrefabs.Where(p =>
                IsAgeUpgradeEligible(p, _ageUpgradeSpawnChance)));

            return eligible;
        }

        private int GetRemainingQuota(GameObject prefab, int maxAllowed)
        {
            return maxAllowed - (_instantiationCounts.TryGetValue(prefab, out var count) ? count : 0);
        }

        private bool PassedChance(float chance)
        {
            return Random.value <= chance;
        }

        private bool IsAgeUpgradeEligible(GameObject prefab, float chance)
        {
            var upgradeSlot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
            if (upgradeSlot == null)
                return false;

            var unitData = GameDataRepository.Instance.FriendlyUnits.GetData(upgradeSlot.Type);
            if (unitData.AgeStage >= upgradeSlot.AgeStage)
                return false;

            return PassedChance(chance);
        }

        public void RegisterInstantiation(GameObject prefab)
        {
            if (_instantiationCounts.ContainsKey(prefab))
                _instantiationCounts[prefab]++;
            else
                _instantiationCounts[prefab] = 1;
        }
    }
}