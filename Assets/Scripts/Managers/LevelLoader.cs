using System;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Project_inspector_Addons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public sealed class LevelLoader : OneInstanceMonoBehaviour<LevelLoader>
    {
        public event Action OnSceneChanged;

        [SerializeField]
        private List<SceneReference> _scenes = new();

        [SerializeField]
        private SceneReference _inGameUiScene;

        [SerializeField]
        private SceneReference _adminUiScene;
        
        [SerializeField]
        private SceneReference _pauseUiScene;
        
        [SerializeField]
        private SceneReference _gameOverUiScene;


        private bool _isUiLoaded = false;
        private bool _isAdminUiLoaded = false;
        private bool _isPauseUiLoaded = false;
        private bool _adminUi;

        private int _currentLevelIndex = 0;
        public int LevelIndex => _currentLevelIndex;

        public List<SceneReference> SceneList => _scenes;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += InitializeOnSceneLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= InitializeOnSceneLoad;
        }

        private void InitializeOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                OnSceneChanged?.Invoke();
            }
        }
        
        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _scenes.Count)
            {
                Debug.Log("No more levels to load.");
                return;
            }
            
            LoadSceneAndResetAdditiveFlags(_currentLevelIndex);
            LoadUiScenes(); 
            GameStates.Instance.StartGame();
        }
        
        
        
        private static void LoadAdditiveScene(SceneReference scene, ref bool isLoaded)
        {
            if (!isLoaded)
            {
                int uiBuildIndex = scene.GetBuildIndex();
                if (uiBuildIndex >= 0)
                {
                    SceneManager.LoadScene(uiBuildIndex, LoadSceneMode.Additive);
                    isLoaded = true;
                }
                else
                {
                    Debug.LogWarning($"{scene} not in Build Settings.");
                }
            }
        }

        private void LoadUiScenes()
        {
            LoadAdditiveScene(_inGameUiScene, ref _isUiLoaded);
            LoadAdditiveScene(_pauseUiScene, ref _isPauseUiLoaded);
            if (_adminUi) 
            {
                LoadAdditiveScene(_adminUiScene, ref _isAdminUiLoaded);
            }
        }
        public void AdminUiToggle(bool isAdmin)
        {
            _adminUi = isAdmin;
        }

        public void LoadMainMenu()
        {
            _adminUi = false;
            LoadSceneAndResetAdditiveFlags(0);
        }

        public void LoadGameOver()
        {
            _adminUi = false;
            _isUiLoaded = false;
            _isAdminUiLoaded = false;
            _isPauseUiLoaded = false;
            SceneManager.LoadScene(_gameOverUiScene.GetBuildIndex());
            _currentLevelIndex = _gameOverUiScene.GetBuildIndex();
        }
        public void LoadSpecificLevel(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex >= _scenes.Count)
            {
                Debug.LogWarning($"Scene index {sceneIndex} is out of bounds.");
                return;
            }

            var selectedScene = _scenes[sceneIndex];
            int buildIndex = selectedScene.GetBuildIndex();
            if (buildIndex > 0)
            {
                LoadSceneAndResetAdditiveFlags(buildIndex);
                LoadUiScenes();
            }
            else if (buildIndex == 0)
            {
                Debug.LogWarning($"use the function - LoadMainMenu()");
            }
            else
            {
                Debug.LogWarning($"Scene '{selectedScene.GetSceneName()}' not found in Build Settings.");
            }
            
        }

        private void LoadSceneAndResetAdditiveFlags(int buildIndex)
        {
            _isUiLoaded = false;
            _isAdminUiLoaded = false;
            _isPauseUiLoaded = false;
            SceneManager.LoadScene(buildIndex);
            _currentLevelIndex = _scenes[buildIndex].GetBuildIndex();
        }
        
        public bool InStartMenu()
        {
            return _currentLevelIndex == 0 || _currentLevelIndex == _gameOverUiScene.GetBuildIndex();
        }
        
        


    }
}














