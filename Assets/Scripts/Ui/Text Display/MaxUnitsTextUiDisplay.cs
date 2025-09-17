using System;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Managers;
using Managers.Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Text_Display
{
    public class MaxUnitsTextUiDisplay : MonoBehaviour
    {
        private static TextMeshProUGUI MaxUnItsText { get; set; }
        private static Image MaxUnitsImage { get; set; }
        

        private void Awake()
        {
            UnitSpawner.OnMaxCapacity += MaxUnitsFlashInRed;
            MaxUnitsImage = GetComponentInChildren<Image>();
            MaxUnItsText =  GetComponentInChildren<TextMeshProUGUI>();
            UpdateMoneyUI();
        }

        private void Start()
        {
            GlobalUnitCounter.Instance.OnCountChanged += UpdateMoneyUI;
        }

        private void OnDestroy()
        {
            UnitSpawner.OnMaxCapacity -= MaxUnitsFlashInRed;
            GlobalUnitCounter.Instance.OnCountChanged -= UpdateMoneyUI;
        }

        private static void UpdateMoneyUI()
        {
            if (!MaxUnItsText)
            {
                return;
            }
            MaxUnItsText.text = $" {GlobalUnitCounter.Instance.FriendlyCount} / {UnitSpawner.Instance.MaxDeployableUnits}";
        }

        private static void MaxUnitsFlashInRed()
        {
            UIEffects.ApplyGraphicFeedback(MaxUnitsImage, changeColor: true, new Color(0.29f, 0.29f, 0.29f, 0.71f), changeScale: true, shakeGraphic: true);
            UIEffects.ApplyGraphicFeedback(MaxUnItsText, changeColor: true, new Color(0.83f, 0f, 0f, 0.88f),changeScale: true, shakeGraphic: true);
        }
    }
}


