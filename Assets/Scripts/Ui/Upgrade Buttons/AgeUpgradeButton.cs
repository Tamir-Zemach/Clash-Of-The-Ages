using Assets.Scripts;
using BackEnd.Economy;
using Managers;
using UnityEngine;

public class AgeUpgradeButton : MonoBehaviour
{
    [SerializeField] private int _ageUpgradeCost;

    public void UpdateAge()
    {
        if (PlayerCurrency.Instance.HasEnoughMoney(_ageUpgradeCost))
        {
            PlayerCurrency.Instance.SubtractMoney(_ageUpgradeCost);
            GameManager.Instance.UpgradePlayerAge();

        }
    }
    
}
