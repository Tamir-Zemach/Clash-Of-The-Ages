using System;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using BackEnd.Utilities.PopupUtil;
using TMPro;
using UnityEngine;

namespace Ui.Buttons.Upgrade_Popup
{
    public class UpgradePopUpText : MonoBehaviour
    {
        [Header("UI Reference")]
        public TextMeshProUGUI UpgradeDescriptionText;

        private UpgradeSlotBase _slot;

        private void Awake()
        {
            _slot = GetComponent<UpgradeSlotBase>();
            UpdateDescriptionText();
        }

        private void UpdateDescriptionText()
        {
            if (_slot == null)
            {
                UpgradeDescriptionText.text = "No upgrade slot found.";
                return;
            }

            switch (_slot.SlotType)
            {
                case SlotType.UnitUpgrade:
                    UpgradeDescriptionText.text = UpgradeDescriptionUtility.GetUnitUpgradeDescription(_slot as UnitUpgradePopupSlot);
                    break;

                case SlotType.AgeUpgrade:
                    UpgradeDescriptionText.text = UpgradeDescriptionUtility.GetAgeUpgradeDescription(_slot as UnitAgeUpgradePopupSlot);
                    break;

                case SlotType.GlobalUpgrade:
                    UpgradeDescriptionText.text = UpgradeDescriptionUtility.GetGlobalUpgradeDescription(_slot as GlobalUpgradePopupSlot);
                    break;

                default:
                    UpgradeDescriptionText.text = "Unsupported slot type.";
                    break;
            }
        }
    }
}