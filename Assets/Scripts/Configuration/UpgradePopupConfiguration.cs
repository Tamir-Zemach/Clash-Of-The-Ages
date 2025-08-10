using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Configuration
{
    public class UpgradePopupConfiguration : OneInstanceMonoBehaviour<UpgradePopupConfiguration>
    {
        public event Action<GameObject> OnUpgradeRegistered; 
        
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
        

        // Public accessors for debugger or other systems
        public List<GameObject> UnitPrefabs => _unitPrefabs;
        public List<GameObject> GlobalPrefabs => _globalPrefabs;
        public List<GameObject> AgeUpgradePrefabs => _ageUpgradePrefabs;
        
        public int MaxInstancesPerUnitPrefab => _maxInstancesPerUnitPrefab;
        
        public int MaxInstancesPerGlobalPrefab => _maxInstancesPerGlobalPrefab;

        protected override void Awake()
        {
            base.Awake();
            UpgradeDataStorage.Instance.OnUnitUpgradeRegistered += GetUnitSlotPrefab;
            UpgradeDataStorage.Instance.OnGlobalUpgradeRegistered += GetGlobalUpgradeSlotPrefab;
            UpgradeDataStorage.Instance.OnAgeUnitUpgradeRegistered += GetAgeUnitUpgradeSlotPrefab;
        }

        private void OnDestroy()
        {
            UpgradeDataStorage.Instance.OnUnitUpgradeRegistered -= GetUnitSlotPrefab;
            UpgradeDataStorage.Instance.OnGlobalUpgradeRegistered -= GetGlobalUpgradeSlotPrefab;
            UpgradeDataStorage.Instance.OnAgeUnitUpgradeRegistered -= GetAgeUnitUpgradeSlotPrefab;
        }

        private void GetUnitSlotPrefab(UnitType unitType, StatType statType)
        {
            var prefab = _unitPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitUpgradePopupSlot>();
                return slot != null && slot.UnitType == unitType && slot.Stat == statType;
            });

            if (prefab != null)
            {
                InvokeSlotPrefab(prefab);   
            }
        }


        private void GetGlobalUpgradeSlotPrefab(UpgradeType upgradeType)
        {
            var prefab = _globalPrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<GlobalUpgradePopupSlot>();
                return slot != null && slot.UpgradeType == upgradeType;
            });

            if (prefab != null)
            {
                InvokeSlotPrefab(prefab);
            }
                
        }


        private void GetAgeUnitUpgradeSlotPrefab(UnitType unitType, AgeStageType ageStage)
        {
            var prefab = _ageUpgradePrefabs.FirstOrDefault(p =>
            {
                var slot = p.GetComponent<UnitAgeUpgradePopupSlot>();
                return slot != null && slot.Type == unitType;
            });

            if (prefab != null)
            {
                InvokeSlotPrefab(prefab);
            }
        }

        private void InvokeSlotPrefab(GameObject slotPrefab)
        {
            OnUpgradeRegistered?.Invoke(slotPrefab);
        }

        public List<GameObject> GetEligiblePrefabs()
        {
            var eligible = new List<GameObject>();
            var attempts = 0;
            const int maxAttempts = 10;

            while (eligible.Count < 3 && attempts < maxAttempts)
            {
                eligible.Clear();

                eligible.AddRange(_unitPrefabs.Where(prefab =>
                    PassedChance(_unitSpawnChance) &&
                    GetRemainingQuota(prefab, _maxInstancesPerUnitPrefab) > 0));

                eligible.AddRange(_globalPrefabs.Where(prefab =>
                    PassedChance(_globalSpawnChance) &&
                    GetRemainingQuota(prefab, _maxInstancesPerGlobalPrefab) > 0));

                eligible.AddRange(_ageUpgradePrefabs.Where(prefab =>
                    IsAgeUpgradeEligible(prefab, _ageUpgradeSpawnChance)));

                // If we already have 3 or more, break early
                if (eligible.Count >= 3)
                    break;

                attempts++;
            }

            // Final fallback: if fewer than 3 eligible prefabs exist in total, return all valid ones
            if (eligible.Count < 3)
            {
                eligible = new List<GameObject>();

                eligible.AddRange(_unitPrefabs.Where(p =>
                    GetRemainingQuota(p, _maxInstancesPerUnitPrefab) > 0));

                eligible.AddRange(_globalPrefabs.Where(p =>
                    GetRemainingQuota(p, _maxInstancesPerGlobalPrefab) > 0));

                eligible.AddRange(_ageUpgradePrefabs.Where(p =>
                    IsAgeUpgradeEligible(p, 1f))); // bypass chance, just check eligibility
            }
            
            return eligible;

        }

        private int GetRemainingQuota(GameObject prefab, int maxAllowed)
        {
            if (_unitPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitUpgradePopupSlot>();
                if (slot == null) return 0;

                int count = UpgradeDataStorage.Instance.GetUnitStatUpgradeCount(slot.UnitType, slot.Stat);
                return maxAllowed - count;
            }

            if (_globalPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                if (slot == null) return 0;

                int count = UpgradeDataStorage.Instance.GetGlobalUpgradeCount(slot.UpgradeType);
                return maxAllowed - count;
            }

            return 0;
        }

        private bool PassedChance(float chance)
        {
            return Random.value <= chance;
        }

        private bool IsAgeUpgradeEligible(GameObject prefab, float chance)
        {
            var slot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
            if (slot == null) return false;

            if (UpgradeDataStorage.Instance.HasAgeUpgrade(slot.Type))
                return false;

            return PassedChance(chance);
        }
    }
}