// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IGameController : IGameComponent
    {
        string CurrentState { get; set; } // This smells...

        bool CheckTransitionAllowedStartingToStarted(double currentElapsedTime);
        bool CheckTransitionAllowedStartedToEnding();

        bool CheckTransitionAllowedEndingToEnded(double currentElapsedTime);
        bool CheckTransitionAllowedEndingToGameOver(double currentElapsedTime);

        void StartGame();
        void EndGame();

        void PauseGame();
        void ResumeGame();
    }
}