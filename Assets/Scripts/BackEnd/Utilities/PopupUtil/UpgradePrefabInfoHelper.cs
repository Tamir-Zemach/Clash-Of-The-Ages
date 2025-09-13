using BackEnd.Base_Classes;
using Configuration;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace BackEnd.Utilities.PopupUtil
{
    public static class UpgradePrefabInfoHelper
    {
        public static string GetCategory(GameObject prefab)
        {
            var config = UpgradePopupConfiguration.Instance;
            if (config.UnitPrefabs.Contains(prefab)) return "Unit";
            if (config.GlobalPrefabs.Contains(prefab)) return "Global";
            if (config.AgeUpgradePrefabs.Contains(prefab)) return "AgeUpgrade";
            if (config.TurretSlotPrefabs.Contains(prefab)) return "Turret";
            return "Unknown";
        }

        public static string GetUpgradeInfo(GameObject prefab)
        {
            var config = UpgradePopupConfiguration.Instance;
            var storage = UpgradeDataStorage.Instance;

            if (config.UnitPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitUpgradePopupSlot>();
                if (slot == null) return "Missing UnitUpgradePopupSlot";
                int count = storage.GetUnitStatUpgradeCount(slot.UnitType, slot.Stat);
                return $"Stat: {slot.Stat}, Count: {count}";
            }

            if (config.GlobalPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                if (slot == null) return "Missing GlobalUpgradePopupSlot";
                int count = storage.GetGlobalUpgradeCount(slot.UpgradeType);
                return $"Global Upgrade: {slot.UpgradeType}, Count: {count}";
            }

            if (config.AgeUpgradePrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
                if (slot == null) return "Missing UnitAgeUpgradePopupSlot";
                bool applied = storage.HasAgeUpgrade(slot.Type);
                return $"Age Upgrade Applied: {applied}";
            }
            if (config.TurretSlotPrefabs.Contains(prefab))
            {
                var slot = prefab.GetComponent<TurretPopUpSlot>();
                if (slot == null) return "Missing TurretPopUpSlot";
                int count = storage.GetTurretUpgradeCount();
                return $"Turret Slot Used: {count}";
            }



            return "Unknown prefab type";
        }

        public static Sprite GetIcon(GameObject prefab)
        {
            var slot = prefab.GetComponent<UpgradeSlotBase>();
            return slot?.Icon;
        }
    }
}