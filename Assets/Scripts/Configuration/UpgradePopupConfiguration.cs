using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using BackEnd.Utilities;
using BackEnd.Utilities.PopupUtil;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Configuration
{
    public class UpgradePopupConfiguration : SingletonMonoBehaviour<UpgradePopupConfiguration>
    {
        public event Action<GameObject> OnUpgradeRegistered;
        public event Action<GameObject> OnPrefabEligibilityChecked;

        [Header("Spawn Chances")]
        [SerializeField] private float _unitSpawnChance = 1f;
        [SerializeField] private float _globalSpawnChance = 1f;
        [SerializeField] private float _ageUpgradeSpawnChance = 1f;

        [Header("Prefabs")]
        [SerializeField] private int _maxInstancesPerUnitPrefab = 3;
        [SerializeField] private List<GameObject> _unitPrefabs;
        [SerializeField] private int _maxInstancesPerGlobalPrefab = 2;
        [SerializeField] private List<GameObject> _globalPrefabs;
        [SerializeField] private List<GameObject> _ageUpgradePrefabs;

        
        public List<GameObject> UnitPrefabs => _unitPrefabs;
        public List<GameObject> GlobalPrefabs => _globalPrefabs;
        public List<GameObject> AgeUpgradePrefabs => _ageUpgradePrefabs;

        public int MaxInstancesPerUnitPrefab => _maxInstancesPerUnitPrefab;
        public int MaxInstancesPerGlobalPrefab => _maxInstancesPerGlobalPrefab;
        public List<GameObject> GetEligiblePrefabs()
        {
            var eligible = new List<GameObject>();
            const int maxAttempts = 10;
            int attempts = 0;

            while (eligible.Count < 3 && attempts++ < maxAttempts)
            {
                eligible.Clear();
                eligible.AddRange(Filter(_unitPrefabs, _unitSpawnChance, SlotType.UnitUpgrade, _maxInstancesPerUnitPrefab));
                eligible.AddRange(Filter(_globalPrefabs, _globalSpawnChance, SlotType.GlobalUpgrade, _maxInstancesPerGlobalPrefab));
                eligible.AddRange(Filter(_ageUpgradePrefabs, _ageUpgradeSpawnChance, SlotType.AgeUpgrade));
            }

            if (eligible.Count < 3)
            {
                eligible.Clear();
                eligible.AddRange(Filter(_unitPrefabs, 1f, SlotType.UnitUpgrade, _maxInstancesPerUnitPrefab));
                eligible.AddRange(Filter(_globalPrefabs, 1f, SlotType.GlobalUpgrade, _maxInstancesPerGlobalPrefab));
                eligible.AddRange(Filter(_ageUpgradePrefabs, 1f, SlotType.AgeUpgrade));
            }

            return eligible;
        }

        private IEnumerable<GameObject> Filter(List<GameObject> prefabs, float chance, SlotType type, int maxAllowed = int.MaxValue)
        {
            foreach (var prefab in prefabs)
            {
                if (Random.value > chance) continue;

                OnPrefabEligibilityChecked?.Invoke(prefab);

                if (UpgradeEligibilityChecker.IsEligible(prefab, type, maxAllowed))
                    yield return prefab;
            }
        }

        public bool IsPrefabIneligible(GameObject prefab)
        {
            var type = UpgradeSlotHelper.GetSlotType(prefab);
            if (type == null) return true;

            int maxAllowed = type switch
            {
                SlotType.UnitUpgrade => _maxInstancesPerUnitPrefab,
                SlotType.GlobalUpgrade => _maxInstancesPerGlobalPrefab,
                _ => int.MaxValue
            };

            return UpgradeEligibilityChecker.IsIneligible(prefab, type.Value, maxAllowed);
        }

        protected override void Awake()
        {
            base.Awake();
            var storage = UpgradeDataStorage.Instance;
            storage.OnUnitUpgradeRegistered += HandleUnitUpgrade;
            storage.OnGlobalUpgradeRegistered += HandleGlobalUpgrade;
            storage.OnAgeUnitUpgradeRegistered += HandleAgeUpgrade;
        }

        private void OnDestroy()
        {
            var storage = UpgradeDataStorage.Instance;
            storage.OnUnitUpgradeRegistered -= HandleUnitUpgrade;
            storage.OnGlobalUpgradeRegistered -= HandleGlobalUpgrade;
            storage.OnAgeUnitUpgradeRegistered -= HandleAgeUpgrade;
        }

        private void HandleUnitUpgrade(UnitType unitType, StatType statType)
        {
            var prefab = _unitPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitUpgradePopupSlot>();
                return slot && slot.UnitType == unitType && slot.Stat == statType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }

        private void HandleGlobalUpgrade(UpgradeType upgradeType)
        {
            var prefab = _globalPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<GlobalUpgradePopupSlot>();
                return slot && slot.UpgradeType == upgradeType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }

        private void HandleAgeUpgrade(UnitType unitType, AgeStageType ageStage)
        {
            var prefab = _ageUpgradePrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitAgeUpgradePopupSlot>();
                return slot && slot.Type == unitType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }
    }
}