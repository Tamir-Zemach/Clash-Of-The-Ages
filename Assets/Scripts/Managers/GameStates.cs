
using System;
using BackEnd.Base_Classes;


namespace Managers
{
    public class GameStates : OneInstanceClass<GameStates>
    {
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameEnded;
        public event Action OnGameReset;
        
        public bool GameIsPlaying { get; private set; }
        public bool GameOver { get; private set; }

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
            GameOver = true;
            OnGameEnded?.Invoke();
            
        }

        public void ResetGameState()
        {
            GameIsPlaying = false;
            GameOver = false;
            OnGameReset?.Invoke();
        }
    }
        
        
        
    }