// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Input.Implementation
{
    using System;
    using Microsoft.Xna.Framework.Input;
    using SpaceFighter.Logic.Input.Interfaces;

    public class InputGamepad : IInput
    {
        public Type DeviceType
        {
            get
            {
                return typeof(GamePad);
            }
        }
    }
}
