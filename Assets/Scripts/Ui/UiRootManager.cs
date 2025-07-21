
using UnityEngine.SceneManagement;
using Assets.Scripts.BackEnd.Enems;
using Assets.Scripts.Ui.TurretButton;
using UnityEngine;
using System;

public class UIRootManager : SceneAwareMonoBehaviour<UIRootManager>
{
    [SerializeField] private CanvasGroup[] _canvasesToShowOnAwake;

    protected override void Awake()
    {
        base.Awake();
    }


    protected override void InitializeOnSceneLoad()
    {
        ResetaAlphaInAllCanvasGroups();
    }


    private void ResetaAlphaInAllCanvasGroups()
    {
        CanvasGroup[] canvasGroups = GetComponentsInChildren<CanvasGroup>();
        for (int i = 0; i < _canvasesToShowOnAwake.Length; i++)
        {
            foreach (var group in canvasGroups)
            {
                if (group != null && group != _canvasesToShowOnAwake[i])
                {
                    group.alpha = 0;
                    group.interactable = false;
                    group.blocksRaycasts = false;
                }

            }
        }


    }



}