using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackEnd.Base_Classes
{
    public abstract class SceneAwareMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            LevelLoader.Instance.OnSceneChanged += HandleSceneLoaded;
        }

        protected virtual void OnDisable()
        {
            LevelLoader.Instance.OnSceneChanged -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded()
        { 
            InitializeOnSceneLoad();
        }

        protected abstract void InitializeOnSceneLoad();
    }
}