using Assets.Scripts;
using BackEnd.Economy;
using TMPro;
using UnityEngine;

public class TextUiDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        UpdateMoneyUI();
        PlayerCurrency.OnMoneyChanged += UpdateMoneyUI;
    }


    public void UpdateMoneyUI()
    {
        if (_moneyText == null)
        {
            return;
        }
        _moneyText.text = $"Money: {PlayerCurrency.Instance.Money}";
    }

}

