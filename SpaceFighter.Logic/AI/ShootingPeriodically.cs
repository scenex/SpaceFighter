// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.AI
{
    using System;

    public class ShootingPeriodically : IShooting
    {
        private double elapsedMilliseconds;

        public void Run(Action action, TimeSpan elapsed)
        {
            this.elapsedMilliseconds += elapsed.TotalMilliseconds;
            
            if (this.elapsedMilliseconds > 1000)
            {
                action.Invoke();
                this.elapsedMilliseconds = 0;
            }            
        }
    }
}
