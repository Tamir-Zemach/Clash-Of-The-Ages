using System;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using BackEnd.Project_inspector_Addons;
using BackEnd.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public sealed class LevelLoader : OneInstanceMonoBehaviour<LevelLoader>
    {
        public event Action OnSceneChanged;

        [Header("Scene References")]
        [SerializeField] private List<SceneReference> _scenes = new();
        [SerializeField] private SceneReference _inGameUiScene;
        [SerializeField] private SceneReference _adminUiScene;
        [SerializeField] private SceneReference _pauseUiScene;
        [SerializeField] private SceneReference _gameOverUiScene;
        [SerializeField] private SceneReference _debuggerUiScene;

        private readonly bool[] _uiFlags = new bool[Enum.GetValues(typeof(UiSceneFlag)).Length];
        private bool _adminUiEnabled;
        private bool _debuggerUiEnabled;

        private int _currentLevelIndex;
        public int LevelIndex => _currentLevelIndex;
        public List<SceneReference> SceneList => _scenes;

        private void OnEnable() => SceneManager.sceneLoaded += HandleSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= HandleSceneLoaded;

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
                OnSceneChanged?.Invoke();
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _scenes.Count)
            {
                Debug.Log("No more levels to load.");
                return;
            }

            Loader.LoadSceneAndResetAdditiveFlags(_scenes[_currentLevelIndex], _uiFlags, out _currentLevelIndex);
            LoadUiScenes();
            GameStates.Instance.StartGame();
        }

        public void LoadMainMenu()
        {
            _adminUiEnabled = false;
            Loader.LoadSceneAndResetAdditiveFlags(_scenes[0], _uiFlags, out _currentLevelIndex);
        }

        public void LoadGameOver()
        {
            _adminUiEnabled = false;
            Array.Fill(_uiFlags, false);
            SceneManager.LoadScene(_gameOverUiScene.GetBuildIndex());
            _currentLevelIndex = _gameOverUiScene.GetBuildIndex();
        }

        public void LoadSpecificLevel(int sceneIndex)
        {
            Loader.LoadSpecificLevel(sceneIndex, _scenes, out _currentLevelIndex, _uiFlags);
            LoadUiScenes();
        }

        public void AdminUiToggle(bool isAdmin) => _adminUiEnabled = isAdmin;
        
        public void DebuggerUiToggle(bool isDebugger) => _debuggerUiEnabled = isDebugger;

        public bool InStartMenu() => _currentLevelIndex == 0 || _currentLevelIndex == _gameOverUiScene.GetBuildIndex();

        private void LoadUiScenes()
        {
            Loader.LoadAdditiveScene(_inGameUiScene, ref _uiFlags[(int)UiSceneFlag.InGame]);
            Loader.LoadAdditiveScene(_pauseUiScene, ref _uiFlags[(int)UiSceneFlag.Pause]);
            Loader.LoadToggledAdditiveScene(_adminUiScene, ref _uiFlags[(int)UiSceneFlag.Admin], _adminUiEnabled);
            Loader.LoadToggledAdditiveScene(_debuggerUiScene, ref _uiFlags[(int)UiSceneFlag.Debugger], _debuggerUiEnabled);
        }
    }
}