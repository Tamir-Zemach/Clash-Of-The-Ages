using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Managers.Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.Text_Display
{
    public class LevelTextDisplay : MonoBehaviour
    {
        private static TextMeshProUGUI LevelText { get; set; }
        private static Image LevelImage { get; set; }

        public Color TextColorToChange;
        public Color ImageColorToChange;


        private void Awake()
        {
            PlayerExp.Instance.OnLevelUp += LevelFlashInYellow;
            PlayerExp.Instance.OnLevelUp += UpdateMoneyUI;
            LevelImage = GetComponentInChildren<Image>();
            LevelText =  GetComponentInChildren<TextMeshProUGUI>();
            UpdateMoneyUI();
        }
        
        private static void UpdateMoneyUI()
        {
            if (!LevelText)
            {
                return;
            }
            LevelText.text = $"Level: {PlayerExp.Instance.Level}";
        }

        private void LevelFlashInYellow()
        {
            UIEffects.ApplyGraphicFeedback(LevelImage,
                changeColor: true, ImageColorToChange,
                changeScale: true ,scaleMultiplier: 3,
                shakeGraphic: true, changeAlpha: true, alpha: 0.6f);
            UIEffects.ApplyGraphicFeedback(LevelText,
                changeColor: true, TextColorToChange,
                changeScale: true ,scaleMultiplier: 3,
                shakeGraphic: true, changeAlpha: true, alpha: 0.6f);
        }
    }
}
