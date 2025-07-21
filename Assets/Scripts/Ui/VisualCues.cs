using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BackEnd.Utilities;
using BackEnd.Economy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Ui
{
    public class VisualCues : PersistentMonoBehaviour<VisualCues>
    {

        [FormerlySerializedAs("_warningImage")] public CanvasGroup WarningImage;
        [FormerlySerializedAs("_healthDropInex")] public CanvasGroup HealthDropInex;
    
        //TODO: deside what is the visual cue when enemy at half health 
        public UnityEvent OnEnemyAtHalfHealth;
    
        private Coroutine _flashingCoroutine;

        private readonly List<Renderer> _activeEnemyRenderers = new();

        private void OnEnable()
        {
            UnitBaseBehaviour.OnSpawned += HandleSpawned;
            PlayerHealth.OnDroppedBelowHalfHealth += IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth += IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth += IndexForEnemyHealthDrop;
        }

        private void OnDisable()
        {
            UnitBaseBehaviour.OnSpawned -= HandleSpawned;
            PlayerHealth.OnDroppedBelowHalfHealth -= IndexForPlayerHealthDrop;
            PlayerHealth.OnHealedAboveHalfHealth -= IndexForPlayerHealed;
            EnemyHealth.OnDroppedBelowHalfHealth -= IndexForEnemyHealthDrop;
        }
        private void HandleSpawned(Renderer rend)
        {
            if (rend != null && !_activeEnemyRenderers.Contains(rend))
                _activeEnemyRenderers.Add(rend);
        }

        private void Update()
        {
            CheckIfEnemiesVisible();
        }

        private void CheckIfEnemiesVisible()
        {

            var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            var anyVisible = _activeEnemyRenderers.Any(rend => rend != null && GeometryUtility.TestPlanesAABB(cameraPlanes, rend.bounds));

            if (!anyVisible && UnitCounter.EnemyCount > 0)
            {
                StartCoroutine(UIEffects.FadeTo(0.7f, WarningImage, 0.3f));
            }
            else
            {
                WarningImage.alpha = 0;
            }
        }


        private void IndexForPlayerHealthDrop()
        {
            _flashingCoroutine = StartCoroutine(UIEffects.CanvasGroupFlashLoopCoroutine(HealthDropInex, 0.5f, 0f, 0.3f, 0.2f, 5));
        }
        private void IndexForPlayerHealed()
        {
            if (_flashingCoroutine != null)
            {
                StopCoroutine(_flashingCoroutine);
                _flashingCoroutine = null;
                HealthDropInex.alpha = 0;
            }
            PlayerHealth.Instance.ResetHealthFlags();
        }

        private void IndexForEnemyHealthDrop()
        {
            OnEnemyAtHalfHealth.Invoke();
        }

    }
}