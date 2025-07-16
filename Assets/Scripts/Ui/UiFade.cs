using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Assets.Scripts.BackEnd.Utilities;

public class UiFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Coroutine currentFade;
    private bool _shouldShow;

    public float fadeDuration = 0.3f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ToggleFade()
    {
        _shouldShow = canvasGroup.alpha <= 0f;
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }
        if (_shouldShow)
        {
            StartCoroutine(MouseRayCaster.Instance.WaitForMouseClick(
                onValidHit: null, onMissedClick: HandleClickFadeOut));
        }
        currentFade = StartCoroutine(UIEffects.FadeTo(_shouldShow ? 1f : 0f, canvasGroup, fadeDuration));
        canvasGroup.interactable = _shouldShow;
        canvasGroup.blocksRaycasts = _shouldShow;
    }

    private void HandleClickFadeOut()
    {
        StartCoroutine(UIEffects.FadeTo(0f, canvasGroup, fadeDuration));
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }






}
