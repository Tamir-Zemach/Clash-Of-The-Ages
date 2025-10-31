
using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using DG.Tweening;
using Managers.Loaders;
using units.Behavior;
using UnityEngine;
using UnityEngine.Events;

namespace VisualCues
{
    public class UiVisualCues : MonoBehaviour
    {
        #region Fields
        

        [Tooltip("Invoked when an enemy drops below half health.")]
        public UnityEvent OnEnemyAtHalfHealth;

        [Tooltip("Invoked when the player drops below half health.")]
        public UnityEvent OnPlayerAtHalfHealth;
        
        private Sequence _flashSequence;
        private Tween _fadeTween;
        
        private CanvasGroup _canvasGroupToFlash;
        private CanvasGroup _canvasGroupToFade;
        

        #endregion

        #region Initialization

        protected void OnEnable()
        {
            LevelLoader.Instance.OnNonAdditiveSceneChanged += InitializeOnNonAdditiveSceneLoad;
            PlayerHealth.OnDroppedBelowHalfHealth += IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth += IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth += IndexForEnemyHealthDrop;
        }

        protected void OnDisable()
        {
            PlayerHealth.OnDroppedBelowHalfHealth -= IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth -= IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth -= IndexForEnemyHealthDrop;
            _canvasGroupToFlash = null;
            _canvasGroupToFade = null;
            
        }

        private void InitializeOnNonAdditiveSceneLoad()
        {
            _fadeTween?.Kill();
            _fadeTween = null;
            _canvasGroupToFlash = null;
            _canvasGroupToFade = null;
        }
        #endregion
        

        #region Methods

        public void FlashCanvasGroup(CanvasGroup canvasGroup)
        {
            _canvasGroupToFlash = canvasGroup;
            _flashSequence = UIEffects.FlashCanvasGroup(_canvasGroupToFlash, 0.2f, 0.7f,onComplete: () =>
            {
                UIEffects.FadeCanvasGroup(_canvasGroupToFlash, 0, 0.3f);
            }); 
        }

        public void FadeCanvasGroup(CanvasGroup canvasGroup)
        {
            _canvasGroupToFade = canvasGroup;
            _fadeTween?.Kill();
            _fadeTween = null;
            _fadeTween = UIEffects.FadeCanvasGroup(_canvasGroupToFade, 0.7f,0.3f);
        }
        

        private void IndexForPlayerHealthDrop()
        {
            OnPlayerAtHalfHealth?.Invoke();
        }

        private void IndexForPlayerHealed()
        {
            _flashSequence?.Kill(true); 
            _flashSequence = null;
            _canvasGroupToFlash?.DOFade(0f, 0.1f);
            _canvasGroupToFlash = null;
            
            PlayerHealth.Instance.ResetHealthFlags();
        }

        private void IndexForEnemyHealthDrop()
        {
            OnEnemyAtHalfHealth?.Invoke();
        }

        #endregion
    }
}