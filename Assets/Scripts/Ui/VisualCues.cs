using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BackEnd.Utilities;
using BackEnd.Economy;
using Managers.Camera;
using UnityEngine;
using UnityEngine.Events;

namespace Ui
{
    public class VisualCues : SceneAwareMonoBehaviour<VisualCues>
    {
        #region Fields

        [Tooltip("Invoked when an enemy is spawned but remains unseen by the camera.")]
        public UnityEvent OnEnemySpawnAndUnseen;

        [Tooltip("Invoked when an enemy drops below half health.")]
        public UnityEvent OnEnemyAtHalfHealth;

        [Tooltip("Invoked when the player drops below half health.")]
        public UnityEvent OnPlayerAtHalfHealth;

        private Coroutine _flashingCoroutine;
        private Coroutine _fadingCoroutine;
        private CanvasGroup _canvasGroupToFlash;
        private CanvasGroup _canvasGroupToFade;

        private readonly List<Renderer> _activeEnemyRenderers = new();
        private Plane[] _cameraPlanes;
        private Camera _camera;
        private bool _checkedEnemyOnce;

        #endregion

        #region Initialization

        protected override void OnEnable()
        {
            base.OnEnable();
            UnitBaseBehaviour.OnSpawned += HandleSpawned;
            PlayerHealth.OnDroppedBelowHalfHealth += IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth += IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth += IndexForEnemyHealthDrop;
            CameraMovement.OnCameraMoved += RecalculateFrustum;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnitBaseBehaviour.OnSpawned -= HandleSpawned;
            PlayerHealth.OnDroppedBelowHalfHealth -= IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth -= IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth -= IndexForEnemyHealthDrop;
            CameraMovement.OnCameraMoved -= RecalculateFrustum;
            
        }

        protected override void InitializeOnSceneLoad()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            _camera = Camera.main;
            StopAllCoroutines();
            _fadingCoroutine = null;
            _checkedEnemyOnce = false;
        }
        private void RecalculateFrustum()
        {
            _cameraPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
        }

        #endregion

        #region Enemy Visibility

        private void HandleSpawned(Renderer rend)
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

                if (_fadingCoroutine != null)
                {
                    _fadingCoroutine = StartCoroutine(UIEffects.FadeTo(0, _canvasGroupToFade, 0.3f));
                }
                return;
            }

            // If no fade is in progress, enemies exist, and we've not yet checked—trigger visual cue for unseen enemies
            if (_fadingCoroutine == null && UnitCounter.EnemyCount > 0 && !_checkedEnemyOnce)
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
            _flashingCoroutine = StartCoroutine(UIEffects.CanvasGroupFlashLoopCoroutine(_canvasGroupToFlash, 0.5f, 0f, 0.3f, 0.2f, 5));
        }

        public void FadeCanvasGroup(CanvasGroup canvasGroup)
        {
            _canvasGroupToFade = canvasGroup;
            if (_fadingCoroutine != null)
            {
                StopCoroutine(_fadingCoroutine);
                _fadingCoroutine = null;
            }
            _fadingCoroutine = StartCoroutine(UIEffects.FadeTo(0.7f, _canvasGroupToFade, 0.3f));
        }
        

        private void IndexForPlayerHealthDrop()
        {
            OnPlayerAtHalfHealth?.Invoke();
        }

        private void IndexForPlayerHealed()
        {
            if (_flashingCoroutine != null)
            {
                StopCoroutine(_flashingCoroutine);
                _flashingCoroutine = null;
                _canvasGroupToFlash.alpha = 0;
                _canvasGroupToFlash = null;
            }
            PlayerHealth.Instance.ResetHealthFlags();
        }

        private void IndexForEnemyHealthDrop()
        {
            OnEnemyAtHalfHealth?.Invoke();
        }

        #endregion
    }
}