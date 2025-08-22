using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace BackEnd.Utilities.EffectsUtil
{
    public static class GameObjectEffects
    {
        private static readonly Dictionary<Transform, Vector3> OriginalScales = new();

        /// <summary>
        /// Applies scale bounce, shake, and optional fade to a GameObject.
        /// </summary>
        public static Sequence ApplyFeedback(
            GameObject target,
            bool bounceScale = true, float scaleMultiplier = 1.2f, float scaleDuration = 0.3f,
            bool shake = false, float shakeDuration = 0.3f,
            bool fade = false, float targetAlpha = 0.5f, float fadeDuration = 0.3f,
            Action onComplete = null)
        {
            if (target == null)
            {
                Debug.LogWarning("ApplyFeedback failed: target GameObject is null.");
                return null;
            }

            Transform transform = target.transform;
            Renderer renderer = target.GetComponent<Renderer>();

            if (!OriginalScales.ContainsKey(transform))
                OriginalScales[transform] = transform.localScale;

            Vector3 originalScale = OriginalScales[transform];
            Sequence sequence = DOTween.Sequence();

            if (bounceScale)
            {
                sequence.Join(transform.DOScale(originalScale * scaleMultiplier, scaleDuration));
                sequence.Append(transform.DOScale(originalScale, scaleDuration));
            }

            if (shake)
            {
                sequence.Join(transform.DOShakeScale(shakeDuration, strength: 0.1f, vibrato: 10, randomness: 90f));
            }

            if (fade && renderer != null && renderer.material.HasProperty("_Color"))
            {
                Color originalColor = renderer.material.color;
                Color fadedColor = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
                sequence.Join(renderer.material.DOColor(fadedColor, fadeDuration));
                sequence.Append(renderer.material.DOColor(originalColor, fadeDuration));
            }

            if (onComplete != null)
                sequence.OnComplete(() => onComplete.Invoke());

            return sequence;
        }

        public static void ShrinkWithMultipliers(
            GameObject target,
            Vector3 baseScale,
            Vector3 growMultiplier,
            Vector3 shrinkMultiplier,
            float growDuration,
            float shrinkDuration,
            Action onComplete = null)
        {
            if (target == null) return;

            Transform transform = target.transform;

            Vector3 growScale = new Vector3(
                baseScale.x * growMultiplier.x,
                baseScale.y * growMultiplier.y,
                baseScale.z * growMultiplier.z);

            Vector3 shrinkScale = new Vector3(
                baseScale.x * shrinkMultiplier.x,
                baseScale.y * shrinkMultiplier.y,
                baseScale.z * shrinkMultiplier.z);

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(growScale, growDuration).SetEase(Ease.OutQuad));
            seq.Append(transform.DOScale(shrinkScale, shrinkDuration).SetEase(Ease.InBack));
            seq.OnComplete(() => onComplete?.Invoke());
        }


        
    }
}