﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    public class PlayerStateEventArgs : EventArgs
    {
        private readonly string oldState;
        private readonly string newState;

        public PlayerStateEventArgs(string oldState, string newState)
        {
            this.oldState = oldState;
            this.newState = newState;
        }

        public string OldState
        {
            get
            {
                return this.oldState;
            }
        }

        public string NewState
        {
            get
            {
                return this.newState;
            }
        }
    }
}
