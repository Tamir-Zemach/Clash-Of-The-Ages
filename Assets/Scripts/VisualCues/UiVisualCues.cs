using System.Collections.Generic;
using System.Linq;
using BackEnd.Economy;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using DG.Tweening;
using Managers;
using Managers.Camera;
using units.Behavior;
using UnityEngine;
using UnityEngine.Events;

namespace VisualCues
{
    public class UiVisualCues : MonoBehaviour
    {
        #region Fields

        [Tooltip("Invoked when an enemy is spawned but remains unseen by the camera.")]
        public UnityEvent OnEnemySpawnAndUnseen;

        [Tooltip("Invoked when an enemy drops below half health.")]
        public UnityEvent OnEnemyAtHalfHealth;

        [Tooltip("Invoked when the player drops below half health.")]
        public UnityEvent OnPlayerAtHalfHealth;
        
        private Sequence _flashSequence;
        private Tween _fadeTween;
        
        private CanvasGroup _canvasGroupToFlash;
        private CanvasGroup _canvasGroupToFade;

        private readonly List<Renderer> _activeEnemyRenderers = new();
        private Plane[] _cameraPlanes;
        private Camera _camera;
        private bool _checkedEnemyOnce;

        #endregion

        #region Initialization

        protected void OnEnable()
        {
            LevelLoader.Instance.OnSceneChanged += InitializeOnSceneLoad;
            UnitBaseBehaviour.OnUnFriendlySpawned += HandleUnFriendlySpawned;
            PlayerHealth.OnDroppedBelowHalfHealth += IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth += IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth += IndexForEnemyHealthDrop;
            CameraMovement.OnCameraMoved += RecalculateFrustum;
        }

        protected void OnDisable()
        {
            UnitBaseBehaviour.OnUnFriendlySpawned -= HandleUnFriendlySpawned;
            PlayerHealth.OnDroppedBelowHalfHealth -= IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth -= IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth -= IndexForEnemyHealthDrop;
            CameraMovement.OnCameraMoved -= RecalculateFrustum;
            _canvasGroupToFlash = null;
            _canvasGroupToFade = null;
            
        }

        private void InitializeOnSceneLoad()
        {
            _camera = Camera.main;
            _fadeTween?.Kill();
            _fadeTween = null;
            _canvasGroupToFlash = null;
            _canvasGroupToFade = null;
            _checkedEnemyOnce = false;
        }
        private void RecalculateFrustum()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
                if (_camera == null)
                {
                    Debug.LogWarning("Camera.main is not assigned yet.");
                    return;
                }
            }

            _cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
        }
        #endregion

        #region Enemy Visibility

        private void HandleUnFriendlySpawned(Renderer rend)
        {
            if (rend && !_activeEnemyRenderers.Contains(rend))
                _activeEnemyRenderers.Add(rend);
        }

        private void Update()
        {
            HandleEnemyVisibilityOnce();
        }

        /// <summary>
        /// Checks whether any enemies are visible to the camera frustum.
        /// If enemies are visible, starts UI fade and prevents further checks.
        /// If enemies are not visible and haven't been checked yet, triggers unseen enemy event.
        /// </summary>
        private void HandleEnemyVisibilityOnce()
        {
            
            if (_checkedEnemyOnce) return;

            // If any enemy is visible, start fade-out effect and mark check as completed
            if (EnemyVisible())
            {
                _checkedEnemyOnce = true;
                _fadeTween?.Kill();
                _fadeTween = null;
                _fadeTween = UIEffects.FadeCanvasGroup(_canvasGroupToFade, 0,0.3f);
                return;
            }

            // If no fade is in progress, enemies exist, and we've not yet checked—trigger visual cue for unseen enemies
            if (_fadeTween == null && UnitCounter.EnemyCount > 0 && !_checkedEnemyOnce)
            {
                OnEnemySpawnAndUnseen?.Invoke();
            }
        }
        private bool EnemyVisible()
        {
            return _activeEnemyRenderers.Any(enemyRend => enemyRend && GeometryUtility.TestPlanesAABB(_cameraPlanes, enemyRend.bounds));
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