using System;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using DG.Tweening;
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
        private Tween _imageTween;
        private Tween _textTween;
        private bool _isImageRed;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }


        public void SlotFlash()
        {
            if (_isImageRed) return;
            _imageTween = UIEffects.ApplyGraphicFeedback(_image, changeColor: true, ImageColorFeedback, changeScale: true, shakeGraphic: true);
            _textTween = UIEffects.ApplyGraphicFeedback(_text, changeColor: true, TextColorFeedback,changeScale: true, shakeGraphic: true);
        }

        public void MarkSlotInRed()
        {
            _isImageRed = true;
            _imageTween?.Kill();
            _textTween?.Kill();

            if (_image != null)
                _image.color = Color.red;

            if (_text != null)
                _text.color = Color.red;
        }
    }
}