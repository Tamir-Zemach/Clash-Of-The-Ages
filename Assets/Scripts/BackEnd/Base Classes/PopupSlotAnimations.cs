using System;
using DG.Tweening;
using Ui.Buttons.Upgrade_Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BackEnd.Base_Classes
{
    public class PopupSlotAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private RectTransform _rectTransform;
        private Vector3 _baseLocalPos;
        private Image _image;
        private Tween _shakeTween;

        private const float Delay = 0.05f;

        [SerializeField] private float _jumpHeight = 30f;
        [SerializeField] private float _jumpDuration = 0.3f;
        [SerializeField] private float _shakeDuration = 0.3f;

        private void Awake()
        {
            UpgradePopup.Instance.OnSlotsSpawned += SlotsSpawned;
        }

        private void SlotsSpawned()
        {
            _baseLocalPos = _rectTransform.localPosition;
        }

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();

            Canvas.ForceUpdateCanvases(); // Ensure layout is settled
            DOVirtual.DelayedCall(Delay, () =>
            {
                _baseLocalPos = _rectTransform.localPosition;
                DoTweenJump();
            });
        }

        private void DoTweenJump()
        {
            _rectTransform.DOLocalMoveY(_baseLocalPos.y + _jumpHeight, _jumpDuration / 2)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _rectTransform.DOLocalMoveY(_baseLocalPos.y, _jumpDuration / 2)
                        .SetEase(Ease.InQuad);
                });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_image != null && _shakeTween == null)
            {
                _shakeTween = _rectTransform.DOShakePosition(
                        duration: _shakeDuration,
                        strength: new Vector3(5f, 5f, 0f),
                        vibrato: 10,
                        randomness: 90f,
                        fadeOut: false)
                    .SetLoops(-1)
                    .SetRelative();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_shakeTween != null)
            {
                _shakeTween.Kill();
                _shakeTween = null;
                _rectTransform.localPosition = _baseLocalPos;
                Debug.Log("Resetting to baseLocalPos: " + _baseLocalPos);
            }
        }
    }
}