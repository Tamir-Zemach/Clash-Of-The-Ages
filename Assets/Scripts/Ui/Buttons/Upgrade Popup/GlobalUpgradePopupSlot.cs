using Assets.Scripts.BackEnd.Enems;
using BackEnd.Data_Getters;
using BackEnd.Economy;
using BackEnd.Utilities;
using Ui.Buttons.Deploy_Button;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui.Buttons.Upgrade_Popup
{
    public class GlobalUpgradePopupSlot: MonoBehaviour
    {
        [FormerlySerializedAs("Type")]
        [Tooltip("Specifies the type of upgrade to apply.")]
        [SerializeField] private UpgradeType _type;

        [Header("Upgrade stats Settings")]
        [Tooltip("Bonus amount added to the stat with each upgrade.")]
        [SerializeField] private int _statBonus;
        

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
            UpgradePopup.Instance.BlockRaycasts(false);
            UIEffects.ShrinkAndDestroy(transform, 1.2f, 0, () =>
            {
                UpgradePopup.Instance.HidePopup();
            });
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