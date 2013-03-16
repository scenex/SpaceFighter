// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IWeaponStrategy
    {
        void Execute(
            Action action, 
            TimeSpan elapsed);

        void Execute(Action firedEvent, 
            double shotIntervalElapsed, 
            IList<IShot> shots, 
            Vector2 shotPosition, 
            float shotRotation, 
            int shotWidth, 
            int shotHeight, 
            Color[] shotColorInformation);
    }
}
