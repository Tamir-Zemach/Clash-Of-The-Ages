using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Text_Display
{
    public class MoneyTextUiDisplay : MonoBehaviour
    {
        private static int _lastMoneyValue;
        private static TextMeshProUGUI MoneyText { get; set; }
        private static Image MoneyImage { get; set; }
        
        private void Awake()
        {
            PlayerCurrency.OnMoneyChanged += UpdateMoneyUI;
            PlayerCurrency.OnDoesntHaveEnoughMoney += MoneyFlashInRed;
            MoneyImage = GetComponentInChildren<Image>();
            MoneyText =  GetComponentInChildren<TextMeshProUGUI>();
            UpdateMoneyUI();
            _lastMoneyValue = PlayerCurrency.Instance.Money;
        }
        
        private static void UpdateMoneyUI()
        {
            if (!MoneyText) return;

            int newMoney = PlayerCurrency.Instance.Money;
            NumberRisingUtil.AnimateNumberRise(MoneyText, _lastMoneyValue, newMoney);
            _lastMoneyValue = newMoney;
        }

        private static void MoneyFlashInRed()
        {
            UIEffects.ApplyGraphicFeedback(MoneyImage, changeColor: true, new Color(0.29f, 0.29f, 0.29f, 0.71f), changeScale: true, shakeGraphic: true);
            UIEffects.ApplyGraphicFeedback(MoneyText, changeColor: true, new Color(0.83f, 0f, 0f, 0.88f),changeScale: true, shakeGraphic: true);
        }

    }
}

