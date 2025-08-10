using System;
using System.Collections.Generic;
using BackEnd.Project_inspector_Addons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackEnd.Utilities
{
    public static class Loader
    {
        public static void LoadSpecificLevel(int sceneIndex, List<SceneReference> scenes, out int currentLevelIndex, bool[] flags = null)
        {
            currentLevelIndex = 0;

            if (sceneIndex < 0 || sceneIndex >= scenes.Count)
            {
                Debug.LogWarning($"Scene index {sceneIndex} is out of bounds.");
                return;
            }

            var selectedScene = scenes[sceneIndex];
            int buildIndex = selectedScene.GetBuildIndex();

            if (buildIndex < 0)
            {
                Debug.LogWarning($"Scene '{selectedScene.GetSceneName()}' not found in Build Settings.");
                return;
            }

            if (buildIndex == 0)
            {
                Debug.LogWarning("Use LoadMainMenu() instead.");
                return;
            }

            LoadSceneAndResetAdditiveFlags(selectedScene, flags, out currentLevelIndex);
        }

        public static void LoadAdditiveScene(SceneReference scene, ref bool isLoaded)
        {
            if (isLoaded || scene == null)
            {
                if (scene == null)
                    Debug.LogWarning("SceneReference is null.");
                return;
            }

            int buildIndex = scene.GetBuildIndex();
            if (buildIndex >= 0)
            {
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
                isLoaded = true;
            }
            else
            {
                Debug.LogWarning($"{scene} not in Build Settings.");
            }
        }

        public static void LoadToggledAdditiveScene(SceneReference scene, ref bool isLoaded, bool toggled)
        {
            if (isLoaded || scene == null || !toggled)
            {
                if (scene == null)
                    Debug.LogWarning("SceneReference is null.");
                return;
            }

            int buildIndex = scene.GetBuildIndex();
            if (buildIndex >= 0)
            {
                SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
                isLoaded = true;
            }
            else
            {
                Debug.LogWarning($"{scene} not in Build Settings.");
            }
        }

        public static void LoadSceneAndResetAdditiveFlags(SceneReference scene, bool[] flags, out int currentLevelIndex)
        {
            if (flags != null)
                Array.Fill(flags, false);

            int buildIndex = scene.GetBuildIndex();
            SceneManager.LoadScene(buildIndex);
            currentLevelIndex = buildIndex;
        }
    }
}