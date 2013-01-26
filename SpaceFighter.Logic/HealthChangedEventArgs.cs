// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    public class HealthChangedEventArgs : EventArgs
    {
        private readonly int newHealth;

        public HealthChangedEventArgs(int newHealth)
        {
            this.newHealth = newHealth;
        }

        public int NewHealth
        {
            get
            {
                return this.newHealth;
            }
        }
    }
}
