
using System;
using Audio;
using BackEnd.Utilities.EffectsUtil;
using DG.Tweening;
using Managers;
using Managers.Loaders;
using UnityEngine;

namespace Ui.Pause
{
    public class PauseMenuButton : MonoBehaviour
    {
        private PauseMenu _pauseMenu;
        private CanvasGroup _canvasGroup;
        private Tween _tween;

        private void Awake()
        {
            if (LevelLoader.Instance == null) return;
            LevelLoader.Instance.OnNonAdditiveSceneChanged += FindComponents;
        }

        private void OnDestroy()
        {
            LevelLoader.Instance.OnNonAdditiveSceneChanged -= FindComponents;
        }

        private void FindComponents()
        {
            _pauseMenu = FindAnyObjectByType<PauseMenu>().GetComponent<PauseMenu>();
            _canvasGroup = _pauseMenu.GetComponent<CanvasGroup>();
        }

        public void OpenOrCloseMenu()
        {
            _tween.Kill();

            if (_pauseMenu.MenuOpen)
            {
                HidePauseMenu();
            }
            else
            {
                ShowPauseMenu();
            }
        }

        private void ShowPauseMenu()
        {
            _tween = UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.3f, onComplete: () =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _pauseMenu.MenuOpen =  true;
                GameStates.Instance.PauseGame();
            });
        }

        private void HidePauseMenu()
        {
            _tween = UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.3f, onComplete: () =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _pauseMenu.MenuOpen =  false;
                GameStates.Instance.StartGame();
            });
        }
        
        
    }
}
