using System;
using Audio;
using BackEnd.Utilities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class StartMenu : MonoBehaviour
    {
        public Toggle AdminUi;
        public Toggle DebuggerUi;
        public void StartGame()
        {
            LevelLoader.Instance.LoadNextLevel();
        }

        public void AdminUiToggle()
        {
            LevelLoader.Instance.AdminUiToggle(AdminUi.isOn);
        }
        
        public void DebuggerUiToggle()
        {
            LevelLoader.Instance.DebuggerUiToggle(DebuggerUi.isOn);
        }
        
        
        public void OnSFXVolumeChanged(Slider sfxSlider)
        {
            AudioSettingsUI.Instance.OnSFXVolumeChanged(sfxSlider);
        }

        public void OnSoundtrackVolumeChange(Slider soundtrackSlider)
        {
            AudioSettingsUI.Instance.OnSoundtrackVolumeChange(soundtrackSlider);
        }

        public void LoadLevel(int levelIndex)
        {
            LevelLoader.Instance.LoadSpecificLevel(levelIndex);
        }

        public void FadeInCanvas(CanvasGroup canvasGroup)
        {
            UIEffects.FadeCanvasGroup(canvasGroup, 1, 0.3f);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        public void FadeOutCanvas(CanvasGroup canvasGroup)
        {
            UIEffects.FadeCanvasGroup(canvasGroup, 0, 0.3f);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        
    }
}
