using BackEnd.Enums;
using BackEnd.Base_Classes;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.InterFaces;
using Ui.Buttons.Deploy_Button;
using Ui.Text_Display;
using UnityEngine;

namespace Ui.Buttons.Upgrade_Buttons
{
    public class PlayerUpgradeButton : ButtonWithCost, IImageSwitchable<UpgradeType>
    {
    
        [Tooltip("Specifies the type of upgrade to apply.")]
        [field: SerializeField] public UpgradeType Type {  get; private set; }

        [Header("Upgrade stats Settings")]
        [Tooltip("Bonus amount added to the stat with each upgrade.")]
        [SerializeField] private int _statBonus;
        

        [Tooltip("Incremental increase in stat upgrade cost after each upgrade")]
        [SerializeField] private int _statCostInc;
    

        public void UpgradeStat()
        {
            if (!PlayerCurrency.Instance.HasEnoughMoney(Cost)) return;
            PlayerCurrency.Instance.SubtractMoney(Cost);
            ApplyUpgrade();
            Cost += _statCostInc;
        }

        private void ApplyUpgrade()
        {
            switch (Type)
            {
                case UpgradeType.MaxHealthIncrease:
                    PlayerHealth.Instance.IncreaseMaxHealth(_statBonus);
                    break;

                case UpgradeType.UnitsCosts:
                    DecreaseCostToAllFriendlyUnits();
                    break;

                case UpgradeType.EnemyMoneyIncrease:
                    IncreaseMoneyGainToAllEnemyUnits();
                    break;
                default:
                    Debug.LogWarning("Unknown upgrade type: " + Type);
                    break;
            }
        }


        private void DecreaseCostToAllFriendlyUnits()
        {
            foreach (var unit in UIObjectFinder.GetButtons<UnitDeployButton, UnitType>())
            {
                unit.Cost -= _statBonus;
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
