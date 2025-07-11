

using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : PersistentMonoBehaviour<LevelLoader>
{
    [SerializeField]
    private List<SceneReference> scenes = new();

    [SerializeField]
    private SceneReference uiScene;
    private bool isUILoaded = false;

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

        // Load UI scene if not already loaded
        if (!isUILoaded)
        {
            int uiBuildIndex = uiScene.GetBuildIndex();
            if (uiBuildIndex >= 0)
            {
                SceneManager.LoadScene(uiBuildIndex, LoadSceneMode.Additive);
                isUILoaded = true;
            }
            else
            {
                Debug.LogWarning("UI Scene not in Build Settings.");
            }
        }

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

    public void ReloadCurrentLevel()
    {
        int buildIndex = scenes[_currentLevelIndex].GetBuildIndex();
        if (buildIndex >= 0)
            SceneManager.LoadScene(buildIndex);
    }

    public SceneReference GetCurrentSceneReference() => scenes[_currentLevelIndex];
}














