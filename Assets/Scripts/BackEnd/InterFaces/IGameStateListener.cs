namespace BackEnd.InterFaces
{
    /// <summary>
    /// Implement this interface to receive game state lifecycle callbacks
    /// (pause, resume, end) from the global GameStates manager.
    /// </summary>
    public interface IGameStateListener
    {
        /// <summary>
        /// Called when the game is paused.
        /// </summary>
        void HandlePause();

        /// <summary>
        /// Called when the game is resumed from a paused state.
        /// </summary>
        void HandleResume();

        /// <summary>
        /// Called when the game ends (win/lose or match termination).
        /// </summary>
        void HandleGameEnd();

    }
}