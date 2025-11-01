using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Enums;
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
        [SerializeField] private float _turretSpawnChance = 1f;

        [Header("Prefabs")]
        [SerializeField] private int _maxInstancesPerUnitPrefab = 3;
        [SerializeField] private List<GameObject> _unitPrefabs;
        [SerializeField] private int _maxInstancesPerGlobalPrefab = 2;
        [SerializeField] private List<GameObject> _globalPrefabs;
        [SerializeField] private List<GameObject> _ageUpgradePrefabs;
        [SerializeField] private int _maxTurretSlots = 2;
        [SerializeField] private List<GameObject> _turretSlotPrefabs;

        // Public accessors for prefab lists and limits
        public List<GameObject> UnitPrefabs => _unitPrefabs;
        public List<GameObject> GlobalPrefabs => _globalPrefabs;
        public List<GameObject> AgeUpgradePrefabs => _ageUpgradePrefabs;
        public List<GameObject> TurretSlotPrefabs => _turretSlotPrefabs;

        public int MaxInstancesPerUnitPrefab => _maxInstancesPerUnitPrefab;
        public int MaxInstancesPerGlobalPrefab => _maxInstancesPerGlobalPrefab;

        /// <summary>
        /// Attempts to build a list of 3 eligible upgrade prefabs based on spawn chances and eligibility rules.
        /// Falls back to guaranteed spawn if not enough are found.
        /// </summary>
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
                eligible.AddRange(Filter(_turretSlotPrefabs, _turretSpawnChance, SlotType.TurretUpgrade, _maxTurretSlots));
            }

            // Fallback: return all eligible prefabs with full chance
            if (eligible.Count >= 3) return eligible;

            eligible.Clear();
            eligible.AddRange(Filter(_unitPrefabs, 1f, SlotType.UnitUpgrade, _maxInstancesPerUnitPrefab));
            eligible.AddRange(Filter(_globalPrefabs, 1f, SlotType.GlobalUpgrade, _maxInstancesPerGlobalPrefab));
            eligible.AddRange(Filter(_ageUpgradePrefabs, 1f, SlotType.AgeUpgrade));
            eligible.AddRange(Filter(_turretSlotPrefabs, 1f, SlotType.TurretUpgrade, _maxTurretSlots));

            return eligible;
        }

        /// <summary>
        /// Filters a list of prefabs based on spawn chance and eligibility rules.
        /// </summary>
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

        /// <summary>
        /// Checks if a prefab is ineligible based on its slot type and max allowed instances.
        /// </summary>
        public bool IsPrefabIneligible(GameObject prefab)
        {
            var type = UpgradeSlotHelper.GetSlotType(prefab);
            if (type == null) return true;

            int maxAllowed = type switch
            {
                SlotType.UnitUpgrade => _maxInstancesPerUnitPrefab,
                SlotType.GlobalUpgrade => _maxInstancesPerGlobalPrefab,
                SlotType.TurretUpgrade => _maxTurretSlots,
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
            storage.OnTurretUpgradeRegistered += HandleTurretUpgrade;
        }
        
        private void OnDestroy()
        {
            var storage = UpgradeDataStorage.Instance;
            storage.OnUnitUpgradeRegistered -= HandleUnitUpgrade;
            storage.OnGlobalUpgradeRegistered -= HandleGlobalUpgrade;
            storage.OnAgeUnitUpgradeRegistered -= HandleAgeUpgrade;
            storage.OnTurretUpgradeRegistered -= HandleTurretUpgrade;
        }

        /// <summary>
        /// Finds and registers the unit upgrade prefab matching the given unit and stat type.
        /// </summary>
        private void HandleUnitUpgrade(UnitType unitType, StatType statType)
        {
            var prefab = _unitPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitUpgradePopupSlot>();
                return slot && slot.UnitType == unitType && slot.Stat == statType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }

        /// <summary>
        /// Finds and registers the global upgrade prefab matching the given upgrade type.
        /// </summary>
        private void HandleGlobalUpgrade(UpgradeType upgradeType)
        {
            var prefab = _globalPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<GlobalUpgradePopupSlot>();
                return slot && slot.UpgradeType == upgradeType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }

        /// <summary>
        /// Finds and registers the age upgrade prefab matching the given unit type.
        /// </summary>
        private void HandleAgeUpgrade(UnitType unitType, AgeStageType ageStage)
        {
            var prefab = _ageUpgradePrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitAgeUpgradePopupSlot>();
                return slot && slot.Type == unitType;
            });

            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }

        /// <summary>
        /// Finds and registers the first available turret upgrade prefab.
        /// </summary>
        private void HandleTurretUpgrade()
        {
            var prefab = _turretSlotPrefabs.FirstOrDefault(p => p.GetComponent<TurretPopUpSlot>() != null);
            if (prefab) OnUpgradeRegistered?.Invoke(prefab);
        }
    }
}