using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd.Base_Classes;
using BackEnd.Enums;
using BackEnd.Project_inspector_Addons;
using BackEnd.Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers.Loaders
{
    public sealed class LevelLoader : SingletonMonoBehaviour<LevelLoader>
    {
        public event Action OnNonAdditiveSceneChanged;

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

        private List<AdditiveSceneInfo> GetAdditiveSceneInfos()
        {
            return new List<AdditiveSceneInfo>
            {
                new(_inGameUiScene, (int)UiSceneFlag.InGame),
                new(_pauseUiScene, (int)UiSceneFlag.Pause),
                new(_adminUiScene, (int)UiSceneFlag.Admin, _adminUiEnabled),
                new(_debuggerUiScene, (int)UiSceneFlag.Debugger, _debuggerUiEnabled)
            };
        }

        
        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _scenes.Count)
            {
                Debug.Log("No more levels to load.");
                return;
            }

            StartCoroutine(LoadSceneWithTransition(_scenes[_currentLevelIndex]));
        }
        
        public void LoadSpecificLevel(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex >= _scenes.Count)
            {
                Debug.LogWarning($"Scene index {sceneIndex} is out of bounds.");
                return;
            }

            StartCoroutine(LoadSceneWithTransition(_scenes[sceneIndex]));
        }

        public void LoadMainMenu()
        {
            _adminUiEnabled = false;
            StartCoroutine(LoadSceneWithTransition(_scenes[0], false));
        }

        public void LoadGameOver()
        {
            _adminUiEnabled = false;
            StartCoroutine(LoadSceneWithTransition(_gameOverUiScene, false));
        }
        
        public void AdminUiToggle(bool isAdmin) => _adminUiEnabled = isAdmin;
        
        public void DebuggerUiToggle(bool isDebugger) => _debuggerUiEnabled = isDebugger;

        public bool InStartMenu() => _currentLevelIndex == 0 || _currentLevelIndex == _gameOverUiScene.GetBuildIndex();
        
        private void ResetUiFlags()
        {
            Array.Fill(_uiFlags, false);
        }
        
        private IEnumerator LoadSceneWithTransition(SceneReference scene, bool loadAdditive = true)
        {
            if (scene == null || LoadingScreenController.Instance == null) yield break;

            int buildIndex = scene.GetBuildIndex();
            if (buildIndex < 0)
            {
                Debug.LogWarning($"Scene '{scene.GetSceneName()}' not found in Build Settings.");
                yield break;
            }

            // 1. Trigger door closing animation
            LoadingScreenController.Instance.StartAnimation();

            // 2. Wait until door is fully closed
            yield return new WaitUntil(() => LoadingScreenController.Instance.IsInState("DoorClosedIdle"));

            // 3. Begin async scene load
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            if (asyncLoad == null) yield break;

            asyncLoad.allowSceneActivation = false;

            // 4. Wait until scene is ready
            yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);

            // 5. Activate scene
            asyncLoad.allowSceneActivation = true;

            // 6. Wait for scene to finish loading
            yield return new WaitUntil(() => asyncLoad.isDone);
            
     

            
            // 7. Load additive scenes AFTER main scene is active
            if (loadAdditive)
            {
                yield return LoadAdditiveScenesAsync(GetAdditiveSceneInfos());
            }
            else
            {
                ResetUiFlags();
            }

            // 8. Trigger door opening animation
            LoadingScreenController.Instance.EndAnimation();
  
            _currentLevelIndex = scene.GetBuildIndex();

            OnNonAdditiveSceneChanged?.Invoke();
            
            // 9. Wait until door is open (optional)
            yield return new WaitUntil(() => LoadingScreenController.Instance.IsInState("DoorIdle"));

            // 10. Update level index and resume game

            if (InStartMenu()) yield break;
            GameStates.Instance.StartGame();
        }
        
        private IEnumerator LoadAdditiveScenesAsync(List<AdditiveSceneInfo> uiScenes)
        {
            List<AsyncOperation> operations = new();

            foreach (var info in uiScenes)
            {

                if (_uiFlags[info.FlagIndex] || info.Scene == null || !info.Toggled) continue;

                int buildIndex = info.Scene.GetBuildIndex();

                if (buildIndex < 0) continue;

                AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                if (op == null) continue;
                operations.Add(op);
                _uiFlags[info.FlagIndex] = true;
            }

            // Wait until all additive scenes are fully loaded
            yield return new WaitUntil(() => operations.TrueForAll(op => op.isDone));
        }
        
    }
}