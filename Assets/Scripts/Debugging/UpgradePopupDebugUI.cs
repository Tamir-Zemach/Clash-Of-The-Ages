using System.Collections.Generic;
using System.Linq;
using BackEnd.Base_Classes;
using BackEnd.Utilities;
using BackEnd.Utilities.PopupUtil;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Configuration;
using Ui.Buttons.Upgrade_Popup;

namespace Debugging
{
    public class UpgradePopupDebugUI : InGameObjectOneInstance<UpgradePopupDebugUI>
    {
        [Header("UI References")]
        [SerializeField] private GameObject _entryPrefab;
        [SerializeField] private Transform _contentParent;

        private UpgradePopupConfiguration _config;
        private readonly Dictionary<GameObject, GameObject> _prefabToEntry = new();

        protected override void Awake()
        {
            base.Awake();
            _config = UpgradePopupConfiguration.Instance;

            UpgradePopup.Instance.OnGettingEligibleList += FlashEligibleSlots;
            _config.OnUpgradeRegistered += UnitUpgradeApplied;
            _config.OnPrefabEligibilityChecked += MarkIneligibleSlots;

            InstantiateAllEntries();
            UpdateSlotTexts();
        }

        private void OnDestroy()
        {
            UpgradePopup.Instance.OnGettingEligibleList -= FlashEligibleSlots;
            _config.OnUpgradeRegistered -= UnitUpgradeApplied;
            _config.OnPrefabEligibilityChecked -= MarkIneligibleSlots;
        }

        private void InstantiateAllEntries()
        {
            var allPrefabs = _config.UnitPrefabs
                .Concat(_config.GlobalPrefabs)
                .Concat(_config.AgeUpgradePrefabs);

            foreach (var prefab in allPrefabs)
            {
                var entry = Instantiate(_entryPrefab, _contentParent);
                var image = entry.GetComponentInChildren<Image>();
                var icon = UpgradePrefabInfoHelper.GetIcon(prefab);
                if (icon != null) image.sprite = icon;

                _prefabToEntry[prefab] = entry;
            }
        }

        private void UpdateSlotTexts()
        {
            foreach (var (prefab, entry) in _prefabToEntry)
            {
                if (entry == null) continue;

                var text = entry.GetComponentInChildren<TMP_Text>();
                if (text == null) continue;

                string category = UpgradePrefabInfoHelper.GetCategory(prefab);
                string info = UpgradePrefabInfoHelper.GetUpgradeInfo(prefab);
                text.text = $"{prefab.name} [{category}]\n{info}";

                if (_config.IsPrefabIneligible(prefab))
                {
                    var visualCue = entry.GetComponent<SlotDebuggerVisualCue>();
                    visualCue?.MarkSlotInRed();
                }
            }
        }

        private void FlashEligibleSlots()
        {
            foreach (var prefab in UpgradePopup.Instance.CurrentEligiblePrefabs)
            {
                if (_prefabToEntry.TryGetValue(prefab, out var entry))
                {
                    entry?.GetComponent<SlotDebuggerVisualCue>()?.SlotFlash();
                }
            }
        }
        private void UnitUpgradeApplied(GameObject upgradedPrefab)
        {
            UpdateSlotTexts();
            if (_prefabToEntry.TryGetValue(upgradedPrefab, out var entry))
            {
                entry?.GetComponent<SlotDebuggerVisualCue>()?.SlotFlash();
            }
        }

        private void MarkIneligibleSlots(GameObject prefab)
        {
            if (_prefabToEntry.TryGetValue(prefab, out var entry))
            {
                if (_config.IsPrefabIneligible(prefab))
                {
                    entry?.GetComponent<SlotDebuggerVisualCue>()?.MarkSlotInRed();
                }
            }
        }



        protected override void HandlePause() { }
        protected override void HandleResume() { }
        protected override void HandleGameEnd() { }

        protected override void HandleGameReset()
        {
            foreach (Transform child in _contentParent)
            {
                Destroy(child.gameObject);
            }
            _prefabToEntry.Clear();
        }
    }
}