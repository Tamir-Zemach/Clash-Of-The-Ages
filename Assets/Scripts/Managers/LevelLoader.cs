

using System;
using System.Collections.Generic;
using BackEnd.Project_inspector_Addons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : SceneAwareMonoBehaviour<LevelLoader>
{
    public event Action OnSceneChanged;
    public Toggle AdminUi;

    [SerializeField]
    private List<SceneReference> _scenes = new();

    [SerializeField]
    private SceneReference _inGameUiScene;

    [SerializeField]
    private SceneReference _adminUiScene;


    private bool _isUiLoaded = false;
    private bool _isAdminUiLoaded = false;
    private bool _adminUi;

    private int _currentLevelIndex = 0;
    public int LevelIndex => _currentLevelIndex;

    public List<SceneReference> SceneList => _scenes;
    protected override void InitializeOnSceneLoad()
    {
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
        
        LoadAdditiveScene(_inGameUiScene, ref _isUiLoaded);
       

        if (_adminUi) 
        {
            LoadAdditiveScene(_adminUiScene, ref _isAdminUiLoaded);
        }

        SceneManager.LoadScene(_currentLevelIndex);
    }
    private void LoadAdditiveScene(SceneReference scene, ref bool isLoaded)
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
    public void AdminUiToggle()
    {
        _adminUi = AdminUi.isOn;
    }

    public void ReloadCurrentLevel()
    {
        int buildIndex = _scenes[_currentLevelIndex].GetBuildIndex();
        if (buildIndex >= 0)
            SceneManager.LoadScene(buildIndex);
    }

    public void LoadSpecificLevel(int sceneIndex)
    {
        if (sceneIndex < 0 || sceneIndex >= _scenes.Count)
        {
            Debug.LogWarning($"Scene index {sceneIndex} is out of bounds.");
            return;
        }

        int buildIndex = _scenes[sceneIndex].GetBuildIndex();
        if (buildIndex >= 0)
            SceneManager.LoadScene(buildIndex);
        else
            Debug.LogWarning($"Scene {sceneIndex} not found in Build Settings.");
    }

    public SceneReference GetCurrentSceneReference() => _scenes[_currentLevelIndex];
    public bool InStartMenu()
    {
        return _currentLevelIndex == 0;
    }


}














