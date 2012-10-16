// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IPlayerWeaponService
    {
        IWeapon Weapon { get; }
        void FireWeapon(Vector2 initialCoordinates);
        void UpgradeWeapon();
        void DowngradeWeapon();
        void RemoveShot(IShot shot);
    }
}
