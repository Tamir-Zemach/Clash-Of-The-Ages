using BackEnd.InterFaces;

namespace BackEnd.Utilities
{
    /// <summary>
    /// Utility class to manage subscription and unsubscription
    /// of IGameStateListener objects to the GameStates events.
    /// </summary>
    public static class GameStateListener
    {
        public static void Subscribe(IGameStateListener listener)
        {
            var gameStates = Managers.GameStates.Instance;
            if (gameStates == null) return;

            gameStates.OnGamePaused += listener.HandlePause;
            gameStates.OnGameResumed += listener.HandleResume;
            gameStates.OnGameEnded += listener.HandleGameEnd;
        }

        public static void Unsubscribe(IGameStateListener listener)
        {
            var gameStates = Managers.GameStates.Instance;
            if (gameStates == null) return;

            gameStates.OnGamePaused -= listener.HandlePause;
            gameStates.OnGameResumed -= listener.HandleResume;
            gameStates.OnGameEnded -= listener.HandleGameEnd;
        }
    }
}