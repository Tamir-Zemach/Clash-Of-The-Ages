using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;
using Managers;

namespace Ui
{
    public class UiFade : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Tween _currentTween;
        private bool _shouldShow;

        [FormerlySerializedAs("fadeDuration")] public float FadeDuration = 0.3f;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ToggleFade()
        {
            _shouldShow = _canvasGroup.alpha <= 0f;

            _currentTween?.Kill();

            if (_shouldShow)
            {
                MouseRayCaster.Instance.WaitForMouseClick(
                    onValidHit: null,
                    onMissedClick: HandleClickFadeOut
                );
            }

            _currentTween = UIEffects.FadeCanvasGroup(_canvasGroup, _shouldShow ? 1f : 0f, FadeDuration, () =>
            {
                _canvasGroup.interactable = _shouldShow;
                _canvasGroup.blocksRaycasts = _shouldShow;
            });
        }

        private void HandleClickFadeOut()
        {
            _currentTween?.Kill();
            _currentTween = UIEffects.FadeCanvasGroup(_canvasGroup, 0f, FadeDuration, () =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            });
        }
    }
}