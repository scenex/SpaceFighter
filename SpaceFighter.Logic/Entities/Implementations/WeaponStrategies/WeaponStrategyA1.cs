// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.WeaponStrategies
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class WeaponStrategyA1 : IWeaponStrategy
    {
        public void Execute(Action action, TimeSpan elapsed)
        {
            action.Invoke();
        }

        public bool Execute(Action firedEvent, double shotIntervalElapsed, IList<IShot> shots, Vector2 shotPosition, float shotRotation, int shotWidth, int shotHeight, Color[] shotColorInformation)
        {
            if (shotIntervalElapsed > 0.1)
            {
                const int Offset = 105 / 2 - 30;

                shots.Add(
                    new ShotA(
                         new Vector2(
                            shotPosition.X - (shotWidth / 2.0f) + Offset * ((float)Math.Cos(shotRotation)),   // Center shot and then add r*cos(angle)
                            shotPosition.Y - (shotHeight / 2.0f) + Offset * ((float)Math.Sin(shotRotation))),  // Center shot and then add r*sin(angle)                   
                        shotWidth,
                        shotHeight,
                        shotColorInformation,
                        20,
                        shotRotation));

                firedEvent.Invoke();
                return true;
            }

            return false;
        }
    }
}
