using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradePopup : MonoBehaviour
    {
        [SerializeField] private GameObject[] _upgradeSlotsPrefabs;

        private void Start()
        {
            InstantiateRandomSlots();
        }

        private void InstantiateRandomSlots()
        {
            if (_upgradeSlotsPrefabs == null || _upgradeSlotsPrefabs.Length < 3)
                return;

            // Shuffle and take 3 unique prefabs
            GameObject[] selectedPrefabs = _upgradeSlotsPrefabs
                .OrderBy(x => Random.value)
                .Take(3)
                .ToArray();

            for (int i = 0; i < selectedPrefabs.Length; i++)
            {
                GameObject slot = Instantiate(selectedPrefabs[i], transform);
            }
        }
    }
}