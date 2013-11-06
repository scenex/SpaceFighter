// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IGameController : IGameComponent
    {
        string CurrentState { get; set; }

        // blablabla

        void FadeIn();
        void FadeOut();

        void StartGame();
        void EndGame();

        void PauseGame();
        void ResumeGame();
    }
}