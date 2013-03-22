// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.WeaponStrategies
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Interfaces;

    public class WeaponStrategyEnemyA : IWeaponStrategy
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

        public bool Execute(Action firedEvent, double shotIntervalElapsed, IList<IShot> shots, Vector2 shotPosition, float shotRotation, int shotWidth, int shotHeight, Color[] shotColorInformation)
        {
            throw new NotImplementedException();
        }
    }
}
