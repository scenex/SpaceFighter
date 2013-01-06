// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    using SpaceFighter.Logic.Input.Interfaces;

    public interface IInputService
    {
        void SetInputDevice(IInput inputDevice);
        void SuspendInputDevice();
        Type InputDeviceType { get; }
    }
}
