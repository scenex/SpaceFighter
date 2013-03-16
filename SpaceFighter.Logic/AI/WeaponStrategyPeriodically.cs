// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.AI
{
    using System;

    public class WeaponStrategyPeriodically : IWeaponStrategy
    {
        private double elapsedMilliseconds;

        public void Execute(Action action, TimeSpan elapsed)
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
