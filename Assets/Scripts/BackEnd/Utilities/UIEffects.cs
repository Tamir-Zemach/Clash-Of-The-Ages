using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BackEnd.Utilities
{
    public static class UIEffects
    {
        #region CanvasGroups

        /// <summary>
        /// Smoothly fades a CanvasGroup's alpha to a target value over a set duration.
        /// </summary>
        public static Tween FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration, Action onComplete = null)
        {
            return canvasGroup.DOFade(targetAlpha, duration)
                .SetEase(Ease.InCirc)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        /// <summary>
        /// Creates a repeating flash effect by fading a CanvasGroup between two alpha levels.
        /// Adjustable parameters for duration, interval, and number of flashes.
        /// </summary>
        public static Sequence FlashCanvasGroup(CanvasGroup canvasGroup,
            float targetAlpha1 = 1f, float targetAlpha2 = 0f,
            float fadeDuration = 0.3f, float timeBetweenFlashes = 0.25f,
            int flashCount = 4, Action onComplete = null)
        {
            Sequence flashSequence = DOTween.Sequence();

            for (int i = 0; i < flashCount; i++)
            {
                flashSequence
                    .Append(canvasGroup.DOFade(targetAlpha1, fadeDuration))
                    .AppendInterval(timeBetweenFlashes)
                    .Append(canvasGroup.DOFade(targetAlpha2, fadeDuration))
                    .AppendInterval(timeBetweenFlashes);
            }

            if (onComplete != null)
                flashSequence.OnComplete(() => onComplete());

            return flashSequence;
        }
        #endregion

        #region Global Generic Graphic Feedback 

        // Stores original colors and scales of Graphic elements to restore them after effects.
        private static readonly Dictionary<Graphic, Color> OriginalColors = new();
        private static readonly Dictionary<Transform, Vector3> OriginalScales = new();
        

        /// <summary>
        /// Applies visual feedback effects to any Graphic UI element (Image, Text, etc.).
        /// Supports dynamic color flash, alpha fade, scale bounce, and shake—all customizable.
        /// </summary>
        public static Sequence ApplyGraphicFeedback(
            Graphic graphic,
            bool changeColor = true, Color colorToChangeTo = default, float colorChangeDuration = 0.3f,
            bool changeScale = true, float scaleMultiplier = 1.1f, float scaleChangeDuration = 0.3f, 
            bool shakeGraphic = false, float shakeDuration = 0.3f,
            bool changeAlpha = false, float alpha = 0, float alphaChangeDuration = 0.3f, 
            Action onComplete = null)
        {
            if (!TryInitGraphic(graphic, out var transform, out var originalColor, out var originalScale))
                return null;

            var sequence = BuildGraphicSequence(
                graphic,
                transform, originalColor, originalScale,
                changeColor, colorToChangeTo, colorChangeDuration,
                changeAlpha, alpha, alphaChangeDuration,
                changeScale, scaleMultiplier, scaleChangeDuration,
                shakeGraphic, shakeDuration
            );

            if (onComplete != null)
                sequence.OnComplete(() => onComplete.Invoke());

            return sequence;
        }

        /// <summary>
        /// Initializes a Graphic element for animation by storing its original color and scale.
        /// Returns false if graphic is null or invalid.
        /// </summary>
        private static bool TryInitGraphic(Graphic graphic, out Transform transform, out Color originalColor, out Vector3 originalScale)
        {
            transform = null;
            originalColor = default;
            originalScale = default;

            if (graphic == null)
            {
                Debug.LogWarning("ApplyGraphicFeedback failed: Graphic is null.");
                return false;
            }

            transform = graphic.transform;

            if (!OriginalColors.ContainsKey(graphic))
                OriginalColors[graphic] = graphic.color;

            if (!OriginalScales.ContainsKey(transform))
                OriginalScales[transform] = transform.localScale;

            originalColor = OriginalColors[graphic];
            originalScale = OriginalScales[transform];
            return true;
        }

        /// <summary>
        /// Assembles a visual effect sequence for a Graphic element.
        /// Builds fade, color, scale, and shake tweens, then restores original states.
        /// </summary>
        private static Sequence BuildGraphicSequence(
            Graphic graphic,
            Transform transform, Color originalColor, Vector3 originalScale,
            bool changeColor, Color colorToChangeTo, float colorDuration,
            bool changeAlpha, float alpha, float alphaDuration,
            bool changeScale, float scaleMultiplier, float scaleDuration,
            bool shakeGraphic, float shakeDuration)
        {
            Sequence sequence = DOTween.Sequence();
            List<Tweener> startTweens = new();
            List<Tweener> endTweens = new();

            if (changeColor)
            {
                startTweens.Add(graphic.DOColor(colorToChangeTo, colorDuration));
                endTweens.Add(graphic.DOColor(originalColor, colorDuration));
            }

            if (changeAlpha)
            {
                startTweens.Add(graphic.DOFade(alpha, alphaDuration));
                endTweens.Add(graphic.DOFade(originalColor.a, alphaDuration));
            }

            if (changeScale)
            {
                startTweens.Add(transform.DOScale(originalScale * scaleMultiplier, scaleDuration));
                endTweens.Add(transform.DOScale(originalScale, scaleDuration));
            }

            if (shakeGraphic)
            {
                startTweens.Add(transform.DOShakeScale(shakeDuration, strength: 0.1f, vibrato: 10, randomness: 90f));
            }

            foreach (var tween in startTweens)
                sequence.Join(tween);

            if (startTweens.Count == 0)
                sequence.AppendInterval(Mathf.Max(colorDuration, alphaDuration, scaleDuration));

            foreach (var tween in endTweens)
                sequence.Append(tween);

            return sequence;
        }

        #endregion

        #region Specific Generic Graphic Feedback

        public static Sequence ShrinkAndDestroy(Transform target, float midScale, float finalScale, Action onComplete = null)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(target.DOScale(midScale, 0.1f))
                .Append(target.DOScale(finalScale, 0.2f));

            if (onComplete != null)
                seq.OnComplete(() => onComplete.Invoke());

            return seq;
        }
        
        public static Tween AnimateSliderFill(Slider slider, float targetValue, float duration, Action onComplete = null)
        {
            return DOTween.To(() => slider.value, x => slider.value = x, targetValue, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        

        #endregion
        

            
    }
}

