using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneAwareMonoBehaviour<T> : PersistentMonoBehaviour<T> where T : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            InitializeOnSceneLoad();
        }
    }

    protected abstract void InitializeOnSceneLoad();
}