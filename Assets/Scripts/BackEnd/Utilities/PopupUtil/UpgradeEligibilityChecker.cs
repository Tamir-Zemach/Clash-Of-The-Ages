using BackEnd.Enums;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace BackEnd.Utilities.PopupUtil
{
    public static class UpgradeEligibilityChecker
    {
        public static bool IsEligible(GameObject prefab, SlotType type, int maxAllowed)
        {
            var storage = UpgradeDataStorage.Instance;

            switch (type)
            {
                case SlotType.UnitUpgrade:
                    var unitSlot = prefab.GetComponent<UnitUpgradePopupSlot>();
                    return unitSlot != null &&
                           storage.GetUnitStatUpgradeCount(unitSlot.UnitType, unitSlot.Stat) < maxAllowed;

                case SlotType.GlobalUpgrade:
                    var globalSlot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                    return globalSlot != null &&
                           storage.GetGlobalUpgradeCount(globalSlot.UpgradeType) < maxAllowed;

                case SlotType.AgeUpgrade:
                    var ageSlot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
                    return ageSlot != null &&
                           !storage.HasAgeUpgrade(ageSlot.Type);
                
                case SlotType.TurretUpgrade:
                    return UpgradeDataStorage.Instance.GetTurretUpgradeCount() < maxAllowed;
                
                default:
                    return false;
            }
        }

        public static bool IsIneligible(GameObject prefab, SlotType type, int maxAllowed)
        {
            return !IsEligible(prefab, type, maxAllowed);
        }
    }
}