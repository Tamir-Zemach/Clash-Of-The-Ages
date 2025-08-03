using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace BackEnd.Base_Classes
{
    public class PopupSlotStartAnim : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Vector2 _originalPos;
        // Delay the animation slightly to ensure layout group has positioned it
        private const float Delay = 0.05f;
        
        [SerializeField] private float _jumpHeight = 30f;
        [SerializeField] private float _jumpDuration = 0.3f;
        
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            
            DoTweenJump();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DoTweenJump();
            }
        }

        private void DoTweenJump()
        {
            // Force layout update before reading anchoredPosition
            Canvas.ForceUpdateCanvases();

            DOVirtual.DelayedCall(Delay, () =>
            {
                _originalPos = _rectTransform.anchoredPosition;
                _rectTransform.DOAnchorPosY(_originalPos.y + _jumpHeight, _jumpDuration / 2)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        _rectTransform.DOAnchorPosY(_originalPos.y, _jumpDuration / 2)
                            .SetEase(Ease.InQuad);
                    });
            });
        }
    }
}