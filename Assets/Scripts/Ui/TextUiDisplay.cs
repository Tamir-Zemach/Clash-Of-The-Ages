using Assets.Scripts;
using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;

public class TextUiDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _enemyhealthText;

    private void Awake()
    {
        UpdateMoneyUI();
        UpdateHealthUI();
        UpdateEnemyHealthUI();
        PlayerCurrency.OnMoneyChanged += UpdateMoneyUI;
        PlayerHealth.OnHealthChanged += UpdateHealthUI;
        EnemyHealth.OnEnemyHealthChanged += UpdateEnemyHealthUI;
    }


    public void UpdateMoneyUI()
    {
        if (_moneyText == null)
        {
            return;
        }
        _moneyText.text = $"Money: {PlayerCurrency.Instance.Money}";
    }

    public void UpdateHealthUI()
    {
        if (_healthText == null)
        {
            return;
        }
        _healthText.text = $"Current health: {PlayerHealth.Instance.CurrentHealth}, Max Health: {PlayerHealth.Instance.MaxHealth}";
    }

    public void UpdateEnemyHealthUI()
    {
        if (_enemyhealthText == null)
        {
            return;
        }
        _enemyhealthText.text = $"Enemy Current health: {EnemyHealth.Instance.CurrentHealth}, Enemy Max Health: {EnemyHealth.Instance.MaxHealth}";
    }



}

