using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace Managers
{
    public class UpgradePopupConfiguration : PersistentMonoBehaviour<UpgradePopupConfiguration>
    {
        [Header("Shared Spawn Chances")]
        [Range(0f, 1f)] [SerializeField] private float _unitSpawnChance = 1f;
        [Range(0f, 1f)] [SerializeField] private float _globalSpawnChance = 1f;
        [Range(0f, 1f)] [SerializeField] private float _ageUpgradeSpawnChance = 1f;

        [Header("Slot Prefabs")]
        [SerializeField] private List<GameObject> _unitPrefabs;
        [SerializeField] private List<GameObject> _globalPrefabs;
        [SerializeField] private List<GameObject> _ageUpgradePrefabs;

        public List<GameObject> GetEligiblePrefabs(bool debug = false)
        {
            var eligible = new List<GameObject>();

            eligible.AddRange(_unitPrefabs.Where(p => PassedChance(_unitSpawnChance, p, debug)));
            eligible.AddRange(_globalPrefabs.Where(p => PassedChance(_globalSpawnChance, p, debug)));
            eligible.AddRange(_ageUpgradePrefabs.Where(p => IsAgeUpgradeEligible(p, _ageUpgradeSpawnChance, debug)));

            return eligible;
        }

        private bool PassedChance(float chance, GameObject prefab, bool debug)
        {
            bool passed = UnityEngine.Random.value <= chance;
            if (debug) Debug.Log($"Prefab {prefab.name} chance: {chance}, passed: {passed}");
            return passed;
        }

        private bool IsAgeUpgradeEligible(GameObject prefab, float chance, bool debug)
        {
            var upgradeSlot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
            if (upgradeSlot == null)
            {
                if (debug) Debug.LogWarning($"Missing UnitAgeUpgradePopupSlot on {prefab.name}");
                return false;
            }

            var unitData = GameDataRepository.Instance.FriendlyUnits.GetData(upgradeSlot.Type);
            if (unitData.AgeStage >= upgradeSlot.AgeStage)
            {
                if (debug) Debug.Log($"Unit {upgradeSlot.Type} already at age {unitData.AgeStage}, skipping.");
                return false;
            }

            return PassedChance(chance, prefab, debug);
        }
    }
    
}
