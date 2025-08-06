using System;
using BackEnd.Base_Classes;
using BackEnd.Data_Getters;
using BackEnd.InterFaces;
using BackEnd.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Ui
{
    public class HoverCostDisplay : OneInstanceMonoBehaviour<HoverCostDisplay>
    {
        public GameObject CostDisplay;
        public Vector2 Offset = new Vector2(15f, -15f); 
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private TextMeshProUGUI _amountText;
        private Canvas _canvas;
        private bool _isTooltipVisible = false;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = CostDisplay.GetComponent<CanvasGroup>();
            _rectTransform  = CostDisplay.GetComponent<RectTransform>();
            _amountText = CostDisplay.GetComponentInChildren<TextMeshProUGUI>();
            _canvas = GetComponent<Canvas>();
        }

        public void ShowTooltip(PointerEventData eventData, int amount, string label = "", Color? color = null)
        {
            _amountText.text = $"{label} {amount}";
            _amountText.color = color ?? Color.white;
            _isTooltipVisible = true;
            UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.3f);
        }

        public void HideTooltip(PointerEventData eventData)
        {
            _isTooltipVisible = false;
            UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.3f);
        }
        
        
        private void Update()
        {
            if (!_isTooltipVisible) return;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out var localPos
            );

            var offsetPos = localPos + Offset;
            _rectTransform.localPosition = Vector2.Lerp(
                _rectTransform.localPosition,
                ClampToCanvasBounds(offsetPos),
                Time.deltaTime * 20f 
            );
            
        }
        public void UpdateCost(int cost)
        {
            if (_isTooltipVisible)
            {
                _amountText.text = "$" + cost;

                // Apply juicy feedback
                UIEffects.ApplyGraphicFeedback(
                    _amountText,
                    changeColor: true,
                    colorToChangeTo: Color.yellow,
                    colorChangeDuration: 0.25f,
                    changeScale: true,
                    scaleMultiplier: 1.2f,
                    shakeGraphic: true,
                    shakeDuration: 0.3f
                );
            }
        }
        
        private Vector2 ClampToCanvasBounds(Vector2 anchoredPos)
        {
            RectTransform canvasRect = _canvas.transform as RectTransform;

            Vector2 min = canvasRect.rect.min + (_rectTransform.rect.size / 2f);
            Vector2 max = canvasRect.rect.max - (_rectTransform.rect.size / 2f);

            float clampedX = Mathf.Clamp(anchoredPos.x, min.x, max.x);
            float clampedY = Mathf.Clamp(anchoredPos.y, min.y, max.y);

            return new Vector2(clampedX, clampedY);
        }
        
        
    }
}