// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    public interface IWeaponService
    {
        IWeapon Weapon { get; }
        void FireWeapon(Vector2 initialCoordinates);
        void UpgradeWeapon();
        void DowngradeWeapon();
    }
}
