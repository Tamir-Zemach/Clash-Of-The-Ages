
using System;
using BackEnd.Base_Classes;
using UnityEngine;


namespace Managers
{
    public class GameStates : Singleton<GameStates>
    {
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameEnded;
        
        public bool GameIsPlaying { get; private set; }

        public void PauseGame()
        {
            GameIsPlaying = false;
            OnGamePaused?.Invoke();
        }

        public void StartGame()
        {
            GameIsPlaying = true;
            OnGameResumed?.Invoke();
        }

        public void EndGame()
        {
            GameIsPlaying = false;
            OnGameEnded?.Invoke();
        }
        
    }
        
        
        
    }