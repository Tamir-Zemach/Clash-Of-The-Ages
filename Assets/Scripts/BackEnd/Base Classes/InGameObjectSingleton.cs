using UnityEngine;

namespace BackEnd.Base_Classes
{
    public abstract class InGameObjectSingleton<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            var gameStates = Managers.GameStates.Instance;

            gameStates.OnGamePaused += HandlePause;
            gameStates.OnGameResumed += HandleResume;
            gameStates.OnGameEnded += HandleGameEnd;
            gameStates.OnGameReset += HandleGameReset;
        }

        protected virtual void OnDisable()
        {
            var gameStates = Managers.GameStates.Instance;
            if (gameStates == null) return;

            gameStates.OnGamePaused -= HandlePause;
            gameStates.OnGameResumed -= HandleResume;
            gameStates.OnGameEnded -= HandleGameEnd;
            gameStates.OnGameReset -= HandleGameReset;
        }

        #region GameLifecycle

        protected abstract void HandlePause();
        protected abstract void HandleResume();
        protected abstract void HandleGameEnd();
        protected abstract void HandleGameReset();

        #endregion
    }
}