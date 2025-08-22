using System;
using BackEnd.Utilities;
using BackEnd.Utilities.EffectsUtil;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Ui
{
    public class PauseMenu : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private bool _isVisible;
        private Tween _tween;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (LevelLoader.Instance.InStartMenu()) return;
            OpenPauseMenuOnEscape();

        }
        
        private void OpenPauseMenuOnEscape()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _tween.Kill();
                
                if (_isVisible)
                {
                    HidePauseMenu();
                }
                else
                {
                    ShowPauseMenu();
                }
            }
        }

        public void ShowPauseMenu()
        {
            
            _tween = UIEffects.FadeCanvasGroup(_canvasGroup, 1, 0.3f, onComplete: () =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                _isVisible =  true;
                GameStates.Instance.PauseGame();
            });
        }

        public void HidePauseMenu()
        {
            _tween = UIEffects.FadeCanvasGroup(_canvasGroup, 0, 0.3f, onComplete: () =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                _isVisible =  false;
                GameStates.Instance.StartGame();
            });
        }
        
        
    }
}
