// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class PlayerHitEventArgs : EventArgs
    {
        public IShot Shot { get; private set; }

        public PlayerHitEventArgs(IShot shot)
        {
            this.Shot = shot;
        }
    }
}
