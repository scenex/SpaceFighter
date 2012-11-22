﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;
    using SpaceFighter.Logic.Input.Interfaces;

    public interface IGameController
    {
        void SetInputDevice(IInput inputDevice);
        Type InputDeviceType { get; }
    }
}
