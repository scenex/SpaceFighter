// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IWeapon // : IEntity?
    {
        event EventHandler<EventArgs> WeaponFired;

        Vector2 Position { get; set; }
        float Rotation { get; set; }       
        IList<IShot> Shots { get; }

        void FireWeapon();
        void UpgradeWeapon();
        void DowngradeWeapon();
    }
}