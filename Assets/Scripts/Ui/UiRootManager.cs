
using UnityEngine.SceneManagement;
using Assets.Scripts.Enems;
using Assets.Scripts.Ui.TurretButton;
using UnityEngine;
using System;

public class UIRootManager : SceneAwareMonoBehaviour<UIRootManager>
{
    public event Action OnSceneChanged;

    protected override void Awake()
    {
        base.Awake();  
    }


    protected override void InitializeOnSceneLoad()
    {
        ResetaAlphaInAllCanvasGroups();
        OnSceneChanged?.Invoke();
    }


    private void ResetaAlphaInAllCanvasGroups()
    {
        CanvasGroup[] canvasGroups = GetComponentsInChildren<CanvasGroup>();

        foreach (var group in canvasGroups)
        {
            if (group != null)
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
        }
    }



}