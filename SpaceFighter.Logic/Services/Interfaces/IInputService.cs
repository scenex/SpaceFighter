// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Input.Interfaces;
    using SpaceFighter.Logic.Services.Implementations;

    public interface IInputService : IGameComponent
    {
        void SetInputDevice(IInput inputDevice);

        bool IsGamePadConnected { get; }
        Type InputDeviceType { get; }

        InputStateHandling InputStateHandling { get; set; }

        bool IsSelectionMoveUp { get; set; }
        bool IsSelectionMoveDown { get; set; }
        bool IsSelectionConfirmed { get; set; }

        void Disable();
        void Enable();
    }
}
