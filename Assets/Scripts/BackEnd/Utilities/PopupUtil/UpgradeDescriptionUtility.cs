using BackEnd.Enums;
using Ui.Buttons.Upgrade_Popup;

namespace BackEnd.Utilities.PopupUtil
{
    public static class UpgradeDescriptionUtility
    {
        public static string GetUnitUpgradeDescription(UnitUpgradePopupSlot unitSlot)
        {
            if (unitSlot == null)
                return "Invalid unit upgrade slot.";

            switch (unitSlot.Stat)
            {
                case StatType.Strength:
                    return $"Increase Strength by {unitSlot.StatBonus}.";

                case StatType.Health:
                    return $"Increase Health by {unitSlot.StatBonus}.";

                case StatType.Range:
                    return $"Increase Range by {unitSlot.StatBonus}.";

                case StatType.AttackSpeed:
                    return $"Increase Attack Speed by {unitSlot.AttackDelayReductionPercent}%.";

                default:
                    return "Unknown stat upgrade.";
            }
        }

        public static string GetAgeUpgradeDescription(UnitAgeUpgradePopupSlot ageSlot)
        {
            if (ageSlot == null)
                return "Invalid age upgrade slot.";

            return
                $"Upgrade {ageSlot.Type} to {ageSlot.AgeStage} stage.\n" +
                $"+{ageSlot.Health} Health\n" +
                $"+{ageSlot.Range} Range\n" +
                $"+{ageSlot.Speed} Speed\n" +
                $"+{ageSlot.MinStrength}-{ageSlot.MaxStrength} Strength\n" +
                $"-{ageSlot.AttackDelayReductionPercent}% Attack Delay";
        }

        public static string GetGlobalUpgradeDescription(GlobalUpgradePopupSlot globalSlot)
        {
            if (globalSlot == null)
                return "Invalid global upgrade slot.";

            switch (globalSlot.UpgradeType)
            {
                case UpgradeType.MaxHealthIncrease:
                    return $"Increase Player Max Health by {globalSlot.StatBonus}.";

                case UpgradeType.UnitsCosts:
                    return $"Reduce cost of all friendly units by {globalSlot.StatBonus}.";

                case UpgradeType.EnemyMoneyIncrease:
                    return $"Increase money gained from enemy units by {globalSlot.StatBonus}.";

                default:
                    return "Unknown global upgrade.";
            }
        }
    }
}