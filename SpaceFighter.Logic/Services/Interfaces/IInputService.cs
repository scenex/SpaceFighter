// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Input.Interfaces;

    public interface IInputService : IGameComponent
    {
        void SetInputDevice(IInput inputDevice);

        bool IsGamePadConnected { get; }
        Type InputDeviceType { get; }

        void Disable();
        void Enable();
    }
}
