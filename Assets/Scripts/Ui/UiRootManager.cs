using System;
using UnityEngine;

namespace Ui
{
    public class UIRootManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] _canvasesToShowOnAwake;
        [SerializeField] private GameObject[] _gameObjectsToDestroyOnLoad;


        private void Awake()
        {
            ResetAlphaInAllCanvasGroups();
            DestroyAllGameObjects();
        }


        private void ResetAlphaInAllCanvasGroups()
        {
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();

            foreach (var group in canvasGroups)
            {
                if (group == null || Array.Exists(_canvasesToShowOnAwake, cg => cg == group)) continue;

                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
        }

        private void DestroyAllGameObjects()
        {
            foreach (var gameObj in _gameObjectsToDestroyOnLoad)
            {
                Destroy(gameObj);
            }
        }
        



    }
}