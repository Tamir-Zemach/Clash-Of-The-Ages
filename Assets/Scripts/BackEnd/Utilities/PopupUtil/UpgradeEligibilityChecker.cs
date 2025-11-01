using BackEnd.Enums;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;

namespace BackEnd.Utilities.PopupUtil
{
    public static class UpgradeEligibilityChecker
    {
        /// <summary>
        /// Determines if a given upgrade prefab is eligible to be shown in the upgrade popup,
        /// based on its type and the number of times it has already been used.
        /// </summary>
        public static bool IsEligible(GameObject prefab, SlotType type, int maxAllowed)
        {
            var storage = UpgradeDataStorage.Instance;

            switch (type)
            {
                case SlotType.UnitUpgrade:
                    // Check if the unit upgrade hasn't exceeded the allowed number of upgrades
                    var unitSlot = prefab.GetComponent<UnitUpgradePopupSlot>();
                    return unitSlot != null &&
                           storage.GetUnitStatUpgradeCount(unitSlot.UnitType, unitSlot.Stat) < maxAllowed;

                case SlotType.GlobalUpgrade:
                    // Check if the global upgrade hasn't exceeded the allowed number of upgrades
                    var globalSlot = prefab.GetComponent<GlobalUpgradePopupSlot>();
                    return globalSlot != null &&
                           storage.GetGlobalUpgradeCount(globalSlot.UpgradeType) < maxAllowed;

                case SlotType.AgeUpgrade:
                    // Check if the age upgrade hasn't already been applied
                    var ageSlot = prefab.GetComponent<UnitAgeUpgradePopupSlot>();
                    return ageSlot != null &&
                           !storage.HasAgeUpgrade(ageSlot.Type);

                case SlotType.TurretUpgrade:
                    // Check if the number of turret upgrades is below the allowed maximum
                    return UpgradeDataStorage.Instance.GetTurretUpgradeCount() < maxAllowed;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the prefab is not eligible for upgrade (inverse of IsEligible).
        /// </summary>
        public static bool IsIneligible(GameObject prefab, SlotType type, int maxAllowed)
        {
            return !IsEligible(prefab, type, maxAllowed);
        }
    }
}