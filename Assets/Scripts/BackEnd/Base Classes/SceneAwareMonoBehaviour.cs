using Managers;
using Managers.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BackEnd.Base_Classes
{
    public abstract class SceneAwareMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            LevelLoader.Instance.OnNonAdditiveSceneChanged += HandleNonAdditiveSceneLoaded;
        }

        protected virtual void OnDisable()
        {
            LevelLoader.Instance.OnNonAdditiveSceneChanged -= HandleNonAdditiveSceneLoaded;
        }

        protected bool IsSceneInitialized { get; private set; }

        private void HandleNonAdditiveSceneLoaded()
        {
            IsSceneInitialized = true;
            InitializeOnSceneLoad();
        }

        protected abstract void InitializeOnSceneLoad();
    }
}