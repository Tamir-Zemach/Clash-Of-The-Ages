using System;
using UnityEngine;

namespace Ui
{
    public class UIRootManager : SceneAwareMonoBehaviour<UIRootManager>
    {
        [SerializeField] private CanvasGroup[] _canvasesToShowOnAwake;
    


        protected override void InitializeOnSceneLoad()
        {
            ResetAlphaInAllCanvasGroups();
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



    }
}