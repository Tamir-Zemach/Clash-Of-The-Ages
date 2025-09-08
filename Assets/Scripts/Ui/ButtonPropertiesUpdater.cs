using System;
using BackEnd.Base_Classes;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class ButtonPropertiesUpdater : MonoBehaviour
    {
        public TextMeshProUGUI CostText;
        private ButtonWithCost _button;
        private int _lastCost;

        private void Awake()
        {
            _button = GetComponentInParent<ButtonWithCost>();
        }



        private void Update()
        {
            int currentCost = _button.Cost;
            if (_lastCost != currentCost)
            {
                CostText.text = currentCost.ToString();
                _lastCost = currentCost;
            }
        }
    }
}