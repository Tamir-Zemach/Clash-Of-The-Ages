using System;
using BackEnd.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Debugging
{
    public class SlotDebuggerVisualCue : MonoBehaviour
    {
        private Image  _image;
        private TextMeshProUGUI _text;
        public Color ImageColorFeedback;
        public Color TextColorFeedback;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }


        public void SlotFlash()
        {
            UIEffects.ApplyGraphicFeedback(_image, changeColor: true, ImageColorFeedback, changeScale: true, shakeGraphic: true);
            UIEffects.ApplyGraphicFeedback(_text, changeColor: true, TextColorFeedback,changeScale: true, shakeGraphic: true);
        }

        public void MarkSlotInRed()
        {
            _image.color = Color.red;
        }
    }
}