using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Managers;

public class PlayerUpgradeButton : MonoBehaviour
{
    [Tooltip("Specifies the type of upgrade to apply.")]
    [SerializeField] private UpgradeType _upgradeType;

    [Header("Upgrade stats Settings")]

    [Tooltip("Bonus amount added to the stat with each upgrade.")]
    [SerializeField] private int _statBonus;

    [Tooltip("Cost to upgrade unit stat")]
    [SerializeField] private int _statUpgradeCost;

    [Tooltip("Incremental increase in stat upgrade cost after each upgrade")]
    [SerializeField] private int _statCostInc;

    public int Cost => _statUpgradeCost;

    public void UpgradeStat()
    {

        if (PlayerCurrency.Instance.HasEnoughMoney(_statUpgradeCost))
        {
            PlayerCurrency.Instance.SubtractMoney(_statUpgradeCost);
            ApplyUpgrade();
            _statUpgradeCost += _statCostInc;
        }
    }

    private void ApplyUpgrade()
    {
        switch (_upgradeType)
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
                Debug.LogWarning("Unknown upgrade type: " + _upgradeType);
                break;
        }
    }


    private void DecreaseCostToAllFriendlyUnits()
    {
        foreach (var unit in GameDataRepository.Instance.FriendlyUnits)
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
