using DG.Tweening;
using TMPro;

namespace BackEnd.Utilities
{
    public static class NumberRisingUtil
    {
        /// <summary>
        /// Animates a number rising from startValue to endValue in a TMP_Text field.
        /// </summary>
        /// <param name="text">The TMP_Text component to update.</param>
        /// <param name="startValue">The initial number.</param>
        /// <param name="endValue">The final number.</param>
        /// <param name="duration">Duration of the animation in seconds.</param>
        public static void AnimateNumberRise(TMP_Text text, int startValue, int endValue, float duration = 0.4f)
        {
            if (text == null || startValue == endValue) return;

            DOTween.Kill(text); // Cancel any ongoing animation on this text

            DOTween.To(() => startValue, x =>
            {
                startValue = x;
                text.text = x.ToString();
            }, endValue, duration);
        }
    }
}