using Assets.Scripts.BackEnd.Utilities;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualCues : PersistentMonoBehaviour<VisualCues>
{

    public CanvasGroup _warningImage;
    public CanvasGroup _healthDropInex;
    private Coroutine _flashingCoroutine;

    private readonly List<Renderer> activeEnemyRenderers = new();

    private void OnEnable()
    {
        UnitBaseBehaviour.OnSpawned += HandleSpawned;
        PlayerHealth.OnDroppedBelowHalfHealth += IndexForPlayerHealthDrop;
        PlayerHealth.OnHealedAboveHalfHealth += IndexForPlayerHealed;
    }

    private void OnDisable()
    {
        UnitBaseBehaviour.OnSpawned -= HandleSpawned;
        PlayerHealth.OnDroppedBelowHalfHealth -= IndexForPlayerHealthDrop;
        PlayerHealth.OnHealedAboveHalfHealth -= IndexForPlayerHealed;
    }
    private void HandleSpawned(Renderer rend)
    {
        if (rend != null && !activeEnemyRenderers.Contains(rend))
            activeEnemyRenderers.Add(rend);
    }

    private void Update()
    {
        CheckIfEnemiesVisible();
    }

    private void CheckIfEnemiesVisible()
    {

        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool anyVisible = false;

        foreach (Renderer rend in activeEnemyRenderers)
        {
            if (rend != null && GeometryUtility.TestPlanesAABB(cameraPlanes, rend.bounds))
            {
                anyVisible = true;
                break;
            }
        }

        if (!anyVisible && UnitCounter.EnemyCount > 0)
        {
            StartCoroutine(UIEffects.FadeTo(0.7f, _warningImage, 0.3f));
        }
        else
        {
            _warningImage.alpha = 0;
        }
    }


    private void IndexForPlayerHealthDrop()
    {
        _flashingCoroutine = StartCoroutine(UIEffects.CanvasGroupFlashLoopCoroutine(_healthDropInex, 0.5f, 0f, 0.3f, 0.2f, 5));
    }
    private void IndexForPlayerHealed()
    {
        if (_flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
            _flashingCoroutine = null;
            _healthDropInex.alpha = 0;
        }
        PlayerHealth.Instance.ResetHealthFlags();
    }


}