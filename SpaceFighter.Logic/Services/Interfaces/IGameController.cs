﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IGameController : IGameComponent
    {
        void StartGame();
        bool IsGameRunning { get; }

        void PauseGame();
        void ResumeGame();
    }
}