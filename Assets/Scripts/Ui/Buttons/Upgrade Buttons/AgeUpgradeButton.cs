using BackEnd.Base_Classes;
using BackEnd.Economy;
using Managers;
using UnityEngine;

namespace Ui.Buttons.Upgrade_Buttons
{
    public class AgeUpgradeButton : ButtonWithCost
    {
        public void UpdateAge()
        {
            if (!PlayerCurrency.Instance.HasEnoughMoney(Cost)) return;
            PlayerCurrency.Instance.SubtractMoney(Cost);
            GameManager.Instance.UpgradePlayerAge();
        }
    
    }
}
