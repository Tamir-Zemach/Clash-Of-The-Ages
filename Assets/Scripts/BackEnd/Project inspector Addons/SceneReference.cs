using System;
using UnityEditor;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneReference
{
    public SceneAsset sceneAsset;

    public string GetScenePath()
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
#else
        return string.Empty;
#endif
    }

    public int GetBuildIndex()
    {
        string path = GetScenePath();
        return SceneUtility.GetBuildIndexByScenePath(path);
    }

    public string GetSceneName()
    {
#if UNITY_EDITOR
        string path = GetScenePath();
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        return fileName;
#else
    return string.Empty;
#endif
    }
    public override string ToString()
    {
        return GetSceneName();
    }

}
