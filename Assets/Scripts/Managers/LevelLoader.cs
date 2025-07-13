

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelLoader : PersistentMonoBehaviour<LevelLoader>
{
    public Toggle AdminUi;

    [SerializeField]
    private List<SceneReference> scenes = new();

    [SerializeField]
    private SceneReference _inGameUiScene;

    [SerializeField]
    private SceneReference _adminUiScene;

    private bool _isUiLoaded = false;
    private bool _isAdminUiLoaded = false;
    private bool _adminUi;

    private int _currentLevelIndex = 0;
    public int LevelIndex => _currentLevelIndex;

    public void LoadNextLevel()
    {
        _currentLevelIndex++;

        if (_currentLevelIndex >= scenes.Count)
        {
            Debug.Log("No more levels to load.");
            return;
        }



        LoadAdditiveScene(_inGameUiScene, _isUiLoaded);

        if (_adminUi) 
        {
            LoadAdditiveScene(_adminUiScene, _isAdminUiLoaded);
        }

        LoadLevelScene();

    }
    private void LoadLevelScene()
    {
        // Load the gameplay level
        int buildIndex = scenes[_currentLevelIndex].GetBuildIndex();
        if (buildIndex >= 0)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogWarning("Gameplay scene not in Build Settings.");
        }
    }
    private void LoadAdditiveScene(SceneReference scene, bool isLoaded)
    {
        // Load scene if not already loaded
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
        int buildIndex = scenes[_currentLevelIndex].GetBuildIndex();
        if (buildIndex >= 0)
            SceneManager.LoadScene(buildIndex);
    }

    public SceneReference GetCurrentSceneReference() => scenes[_currentLevelIndex];
}














