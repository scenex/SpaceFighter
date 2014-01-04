// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework.Input;

    public class GamePadStateEventArgs : EventArgs
    {
        public GamePadStateEventArgs(GamePadState gamePadState)
        {
            this.GamePadState = gamePadState;
        }

        public GamePadState GamePadState { get; private set; }
    }
}
