// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.WeaponStrategies
{
    using System;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class WeaponStrategyA2 : IWeaponStrategy
    {
        public void Execute(Action action, TimeSpan elapsed)
        {
            action.Invoke();
        }
    }
}
