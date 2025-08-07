using System.Linq;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace Debugging
{
    public class UpgradeDataDebugger : MonoBehaviour
    {
        [SerializeField] private bool _debug;

        private void Update()
        {
            if (_debug)
            {
                PrintUpgradeData();
            }
        }

        private void PrintUpgradeData()
        {
            var storage = UpgradeDataStorage.Instance;
            
            if (storage.UnitStatUpgrades.Count == 0)
            {
                Debug.Log("  (none)");
            }
            else
            {
                foreach (var unitEntry in storage.UnitStatUpgrades)
                {
                    string upgrades = string.Join(", ", unitEntry.Value.Select(stat => $"{stat.Key} × {stat.Value}"));
                    Debug.Log($"{unitEntry.Key}: {upgrades}");
                }
            }
            
            if (storage.GlobalUpgrades.Count == 0)
            {
                Debug.Log("  (none)");
            }
            else
            {
                string globalUpgrades = string.Join(", ", storage.GlobalUpgrades.Select(g => $"{g.Key} × {g.Value}"));
                Debug.Log(globalUpgrades);
            }
            if (storage.AgeUpgrades.Count == 0)
            {
                Debug.Log("  (none)");
            }
            else
            {
                foreach (var ageEntry in storage.AgeUpgrades)
                {
                    Debug.Log($"{ageEntry.Key} → {ageEntry.Value}");
                }
            }
        }
    }
}