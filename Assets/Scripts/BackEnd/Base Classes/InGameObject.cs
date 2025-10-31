using BackEnd.InterFaces;
using BackEnd.Utilities;
using UnityEngine;

namespace BackEnd.Base_Classes
{
    public abstract class InGameObject : MonoBehaviour, IGameStateListener
    {
        protected virtual void OnEnable()
        {
            GameStateListener.Subscribe(this);
        }

        protected virtual void OnDisable()
        {
            GameStateListener.Unsubscribe(this);
        }

        public abstract void HandlePause();
        public abstract void HandleResume();
        public abstract void HandleGameEnd();
    }
}