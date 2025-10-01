using System;
using System.Collections;
using Audio;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using Managers;
using Managers.Loaders;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class StartMenu : MonoBehaviour
    {
        public Toggle AdminUi;
        public Toggle DebuggerUi;

        public CanvasGroup Buttons;
        public CanvasGroup LevelSelectionPanel;
        
        public void AdminUiToggle()
        {
            LevelLoader.Instance.AdminUiToggle(AdminUi.isOn);
        }
        
        public void DebuggerUiToggle()
        {
            LevelLoader.Instance.DebuggerUiToggle(DebuggerUi.isOn);
        }
        

        public void LoadLevel(int levelIndex)
        {
            LevelLoader.Instance.LoadSpecificLevel(levelIndex);
        }
        

        public void ToggleCanvasGroups(bool showLevelSelection)
        {
            StartCoroutine(TransitionCanvasGroups(showLevelSelection ? LevelSelectionPanel : Buttons, showLevelSelection ? Buttons : LevelSelectionPanel));
        }
        
        private IEnumerator TransitionCanvasGroups(CanvasGroup toShow, CanvasGroup toHide)
        {
            LoadingScreenController.Instance.StartAnimation();
            yield return new WaitUntil(() => LoadingScreenController.Instance.IsInState("DoorClosedIdle"));

            toShow.alpha = 1;
            toShow.interactable = true;
            toShow.blocksRaycasts = true;

            toHide.alpha = 0;
            toHide.interactable = false;
            toHide.blocksRaycasts = false;

            LoadingScreenController.Instance.EndAnimation();
        }
        
    }
}
