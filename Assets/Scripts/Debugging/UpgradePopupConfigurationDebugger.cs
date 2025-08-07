using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Debugging
{
    public class UpgradePopupConfigurationDebugger : MonoBehaviour
    {
        [SerializeField] private bool _debugInGeneral;
        [SerializeField] private bool _logUnitCounts;
        [SerializeField] private bool _logGlobalCounts;
        [SerializeField] private bool _logAgeUpgradeCounts;
        [SerializeField] private bool _logSummary;

      

        private void Update()
        {
            if (_debugInGeneral)
            {
                LogInstantiationStats();
            }
        }

        private void LogInstantiationStats()
        {
            var config = Configuration.UpgradePopupConfiguration.Instance;
            var counts = config.InstantiationCounts;

            if (_logUnitCounts)
                LogCategory("Unit Prefabs", config.UnitPrefabs, counts);

            if (_logGlobalCounts)
                LogCategory("Global Prefabs", config.GlobalPrefabs, counts);

            if (_logAgeUpgradeCounts)
                LogCategory("Age Upgrade Prefabs", config.AgeUpgradePrefabs, counts);

            if (_logSummary)
                Debug.Log($"Total Instantiated Prefabs: {counts.Values.Sum()}");
        }

        private void LogCategory(string label, List<GameObject> prefabs, IReadOnlyDictionary<GameObject, int> counts)
        {
            Debug.Log($"--- {label} ---");
            foreach (var prefab in prefabs)
            {
                int count = counts.TryGetValue(prefab, out var c) ? c : 0;
                Debug.Log($"{prefab.name}: {count}");
            }
        }
    }
}