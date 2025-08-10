using BackEnd.Enums;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace BackEnd.Utilities.PopupUtil
{
    public static class UpgradeSlotHelper
    {
        public static SlotType? GetSlotType(GameObject prefab)
        {
            if (prefab.GetComponent<UnitUpgradePopupSlot>()) return SlotType.UnitUpgrade;
            if (prefab.GetComponent<GlobalUpgradePopupSlot>()) return SlotType.GlobalUpgrade;
            if (prefab.GetComponent<UnitAgeUpgradePopupSlot>()) return SlotType.AgeUpgrade;
            return null;
        }
    }
}