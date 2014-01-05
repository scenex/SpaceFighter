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
        event EventHandler<GamePadStateEventArgs> AnalogMoveChanged;
        event EventHandler<GamePadStateEventArgs> AnalogFireChanged;
        event EventHandler<GamePadStateEventArgs> AnalogPauseChanged;

        event EventHandler MoveUpChanged;
        event EventHandler MoveDownChanged;
        event EventHandler MoveLeftChanged;
        event EventHandler MoveRightChanged;
        event EventHandler FireChanged;
        event EventHandler PauseChanged;

        event EventHandler MenuSelectionUpChanged;
        event EventHandler MenuSelectionDownChanged;
        event EventHandler MenuSelectionConfirmedChanged;

        bool IsGamePadConnected { get; }
        void SetInputDevice(IInput inputDevice);
        InputStateHandling InputStateHandling { get; set; }
    }
}