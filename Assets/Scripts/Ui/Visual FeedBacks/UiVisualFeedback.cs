using BackEnd.Enums;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui.Visual_FeedBacks
{
    public class UiVisualFeedBack : MonoBehaviour
    {
        [Header("Effect Preset")]
        public UIEffectType SelectedEffect = UIEffectType.None;
        public bool UseCustomParameters = false;

        [Header("Custom Parameters")]
        public Color CustomColor = Color.red;
        public float ColorDuration = 0.3f;

        public float ScaleMultiplier = 1.1f;
        public float ScaleDuration = 0.3f;

        public float Alpha = 0.5f;
        public float AlphaDuration = 0.3f;

        public float ShakeDuration = 0.3f;

        public void PlayEffect()
        {
            switch (SelectedEffect)
            {
                case UIEffectType.None:
                    break;

                case UIEffectType.ShrinkAndDestroy:
                    UIEffects.ShrinkAndDestroy(transform, 0.7f, 0f);
                    break;

                case UIEffectType.FlashRed:
                    UIEffects.ApplyGraphicFeedback(
                        GetComponent<Graphic>(),
                        changeColor: true, colorToChangeTo: Color.red, colorChangeDuration: 0.2f,
                        changeAlpha: true, alpha: 0.2f, alphaChangeDuration: 0.2f,
                        changeScale: false, scaleMultiplier: 1f, scaleChangeDuration: 0f,
                        shakeGraphic: false, shakeDuration: 0f
                    );
                    break;

                case UIEffectType.BounceScale:
                    UIEffects.ApplyGraphicFeedback(
                        GetComponent<Graphic>(),
                        changeColor: false,
                        changeAlpha: false,
                        changeScale: true, scaleMultiplier: 1.3f, scaleChangeDuration: 0.2f,
                        shakeGraphic: false, shakeDuration: 0f
                    );
                    break;

                case UIEffectType.FadeOut:
                    UIEffects.FadeCanvasGroup(GetComponent<CanvasGroup>(), 0f, 0.5f);
                    break;
            }

            if (UseCustomParameters)
            {
                UIEffects.ApplyGraphicFeedback(
                    GetComponent<Graphic>(),
                    changeColor: true, colorToChangeTo: CustomColor, colorChangeDuration: ColorDuration,
                    changeAlpha: true, alpha: Alpha, alphaChangeDuration: AlphaDuration,
                    changeScale: true, scaleMultiplier: ScaleMultiplier, scaleChangeDuration: ScaleDuration,
                    shakeGraphic: true, shakeDuration: ShakeDuration
                );
            }
        }
    }
}