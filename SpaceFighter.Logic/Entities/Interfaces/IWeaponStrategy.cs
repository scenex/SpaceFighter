// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;

    public interface IWeaponStrategy
    {
        void Execute(Action action, TimeSpan elapsed);
    }
}
