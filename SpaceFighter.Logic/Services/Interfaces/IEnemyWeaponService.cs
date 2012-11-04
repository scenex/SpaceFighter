// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IEnemyWeaponService
    {
        IWeapon Weapon { get; }
        void FireWeapon(Vector2 initialCoordinates, double angle);
    }
}
