
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.BackEnd.Utilities
{
    public static class UIEffects
    {

        public static IEnumerator FadeTo(float targetAlpha, CanvasGroup canvasGroup, float fadeDuration)
        {
            float startAlpha = canvasGroup.alpha;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }

        
        public static IEnumerator CanvasGroupFlashLoopCoroutine(
            CanvasGroup canvasGroup,
            float targetAlpha1,
            float targetAlpha2,
            float fadeDuration,
            float timeBetweenFlashes,
            int flashCount)
        {
            for (int i = 0; i < flashCount; i++)
            {
                yield return FadeTo(targetAlpha1, canvasGroup, fadeDuration);
                yield return new WaitForSeconds(timeBetweenFlashes);
                yield return FadeTo(targetAlpha2, canvasGroup, fadeDuration);
                yield return new WaitForSeconds(timeBetweenFlashes);
            }
        }

        
        public static IEnumerator CanvasGroupFlashLoopCoroutine(CanvasGroup canvasGroup)
        {
            return CanvasGroupFlashLoopCoroutine(
                canvasGroup,
                1f,           // targetAlpha1
                0f,           // targetAlpha2
                0.3f,         // fadeDuration
                0.25f,        // timeBetweenFlashes
                4             // flashCount
            );
        }

        public static IEnumerator CanvasGroupFlashLoopCoroutine(CanvasGroup canvasGroup, float timeBetweenFlashes, int flashCount)
        {
            return CanvasGroupFlashLoopCoroutine(
                canvasGroup,
                1f,           // targetAlpha1
                0f,           // targetAlpha2
                0.3f,         // fadeDuration
                timeBetweenFlashes,
                flashCount
            );
        }






    }
}
