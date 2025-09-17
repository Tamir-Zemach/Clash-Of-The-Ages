using BackEnd.Base_Classes;
using BackEnd.Enums;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Buttons.Upgrade_Popup
{
    public class GlobalUpgradePopupSlot: UpgradeSlotBase
    {
        [FormerlySerializedAs("Type")]
        [Tooltip("Specifies the type of upgrade to apply.")]
        [SerializeField] private UpgradeType _type;

        [Header("Upgrade stats Settings")]
        [Tooltip("Bonus amount added to the stat with each upgrade.")]
        [SerializeField] private int _statBonus;

        public UpgradeType UpgradeType => _type;
        
        public int StatBonus => _statBonus;
        
        public override SlotType SlotType => SlotType.GlobalUpgrade;
        
        

        public void ApplyUpgrade()
        {
            switch (_type)
            {
                
                case UpgradeType.MaxHealthIncrease:
                    PlayerHealth.Instance.IncreaseMaxHealth(_statBonus);
                    PlayerHealth.Instance.FullHealth();
                    break;

                case UpgradeType.UnitsCosts:
                    DecreaseCostToAllFriendlyUnits();
                    break;

                case UpgradeType.EnemyMoneyIncrease:
                    IncreaseMoneyGainToAllEnemyUnits();
                    break;
                default:
                    Debug.LogWarning("Unknown upgrade type: " + _type);
                    break;
            }

            FinalizeUpgrade();
        }

        private void FinalizeUpgrade()
        {
            UpgradeDataStorage.Instance.RegisterGlobalUpgrade(_type);
            UpgradePopup.Instance.BlockRaycasts(false);
            UIEffects.ShrinkAndDestroy(transform, 1.2f, 0, () =>
            {
                UpgradePopup.Instance.HidePopup();
            });
        }


        private void DecreaseCostToAllFriendlyUnits()
        {
            var deployButtons = UIObjectFinder.GetButtons<UnitDeployButton, UnitType>();
            foreach (var deployButton in deployButtons)
            {
                deployButton.Cost -= _statBonus;
            }

        }
        private void IncreaseMoneyGainToAllEnemyUnits()
        {
            foreach (var unit in GameDataRepository.Instance.EnemyUnits)
            {
                unit.MoneyWhenKilled += _statBonus;
            }
        }
    }
}