// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.AI
{
    using System;

    public class ShootingPeriodically : IShooting
    {
        public void Run(Action action)
        {
            action.Invoke();
        }
    }
}
