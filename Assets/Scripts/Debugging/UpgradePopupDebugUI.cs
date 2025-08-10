using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Configuration;
using Ui.Buttons.Upgrade_Popup;

namespace Debugging
{
    public class UpgradePopupDebugUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject _entryPrefab;
        [SerializeField] private Transform _contentParent;

        private UpgradePopupConfiguration _config;
        private readonly Dictionary<GameObject, GameObject> _prefabToEntry = new();
        private List<GameObject> _allPrefabs;

        private void Awake()
        {
            _config = UpgradePopupConfiguration.Instance;
            UpgradePopup.Instance.OnGettingEligibleList += FlashEligibleSlots;
            UpgradePopupConfiguration.Instance.OnUpgradeRegistered += UnitUpgradeApplied;

            InstantiateAllEntries();
            UpdateSlotTexts();
        }

        private void OnDestroy()
        {
            UpgradePopup.Instance.OnGettingEligibleList -= FlashEligibleSlots;
            UpgradePopupConfiguration.Instance.OnUpgradeRegistered += UnitUpgradeApplied;
        }

        private void InstantiateAllEntries()
        {
            _allPrefabs = new List<GameObject>();
            _allPrefabs.AddRange(_config.UnitPrefabs);
            _allPrefabs.AddRange(_config.GlobalPrefabs);
            _allPrefabs.AddRange(_config.AgeUpgradePrefabs);

            foreach (var prefab in _allPrefabs)
            {
                var entry = Instantiate(_entryPrefab, _contentParent);
                var entryImage = entry.GetComponentInChildren<Image>();
                var sprite = GetSpriteFromPrefab(prefab);

                if (sprite != null)
                {
                    entryImage.sprite = sprite;
                }

                _prefabToEntry[prefab] = entry;
            }
        }

        private Sprite GetSpriteFromPrefab(GameObject prefab)
        {
            var slot = prefab.GetComponent<UpgradeSlotBase>();
            if (slot == null)
            {
                Debug.LogWarning($"Prefab {prefab.name} is missing UpgradeSlotBase");
                return null;
            }

            return slot.Icon;
        }

        private void FlashEligibleSlots()
        {
            var eligiblePrefabs = UpgradePopup.Instance.CurrentEligiblePrefabs;

            foreach (var prefab in eligiblePrefabs)
            {
                if (_prefabToEntry.TryGetValue(prefab, out var entry))
                {
                    var visualCue = entry.GetComponent<SlotDebuggerVisualCue>();
                    if (visualCue != null)
                    {
                        visualCue.SlotFlash();
                    }
                }
            }
        }

        private void UpdateSlotTexts()
        {
            foreach (var kvp in _prefabToEntry)
            {
                var prefab = kvp.Key;
                var entry = kvp.Value;

                string category = GetCategory(prefab);
                string info = GetUpgradeInfo(prefab);

                var text = entry.GetComponentInChildren<TMP_Text>();
                text.text = $"{prefab.name} [{category}]\n{info}";
                
                MarkIneligibleSlot(prefab, entry);
            }
        }

        private void MarkIneligibleSlot(GameObject prefab, GameObject entry)
        {
            var visualCue = entry.GetComponent<SlotDebuggerVisualCue>();
            if (visualCue == null) return;

            bool isIneligible = false;

            if (_config.UnitPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitUpgradePopupSlot>();
                if (slot != null)
                {
                    int count = UpgradeDataStorage.Instance.GetUnitStatUpgradeCount(slot.UnitType, slot.Stat);
                    isIneligible = count >= _config.MaxInstancesPerUnitPrefab;
                }
            }
            else if (_config.GlobalPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                if (slot != null)
                {
                    int count = UpgradeDataStorage.Instance.GetGlobalUpgradeCount(slot.UpgradeType);
                    isIneligible = count >= _config.MaxInstancesPerGlobalPrefab;
                }
            }
            else if (_config.AgeUpgradePrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
                if (slot != null)
                {
                    isIneligible = UpgradeDataStorage.Instance.HasAgeUpgrade(slot.Type);
                }
            }

            if (isIneligible)
            {
                visualCue.MarkSlotInRed();
                var text = entry.GetComponentInChildren<TMP_Text>();
                text.text += "\n<color=red>Not eligible</color>";
            }
        }

        private string GetCategory(GameObject prefab)
        {
            if (_config.UnitPrefabs.Contains(prefab)) return "Unit";
            if (_config.GlobalPrefabs.Contains(prefab)) return "Global";
            if (_config.AgeUpgradePrefabs.Contains(prefab)) return "AgeUpgrade";
            return "Unknown";
        }

        private string GetUpgradeInfo(GameObject prefab)
        {
            if (_config.UnitPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitUpgradePopupSlot>();
                if (slot == null) return "Missing UnitUpgradePopupSlot";

                int count = UpgradeDataStorage.Instance.GetUnitStatUpgradeCount(slot.UnitType, slot.Stat);
                return $"Stat: {slot.Stat}, Count: {count}";
            }

            if (_config.GlobalPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                if (slot == null) return "Missing GlobalUpgradePopupSlot";

                int count = UpgradeDataStorage.Instance.GetGlobalUpgradeCount(slot.UpgradeType);
                return $"Global Upgrade: {slot.UpgradeType}, Count: {count}";
            }

            if (_config.AgeUpgradePrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
                if (slot == null) return "Missing UnitAgeUpgradePopupSlot";

                bool applied = UpgradeDataStorage.Instance.HasAgeUpgrade(slot.Type);
                return $"Age Upgrade Applied: {applied}";
            }

            return "Unknown prefab type";
        }

        private void UnitUpgradeApplied(GameObject upgradedPrefab)
        {
            UpdateSlotTexts();

            if (_prefabToEntry.TryGetValue(upgradedPrefab, out var entry))
            {
                var visualCue = entry.GetComponent<SlotDebuggerVisualCue>();
                if (visualCue != null)
                {
                    visualCue.SlotFlash();
                }
            }
        }
    }
}