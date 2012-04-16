// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The interface to represent the player's spaceship
    /// </summary>
    public interface ISpaceship
    {
        event EventHandler CollisionDetected;

        Vector2 Position { get; }

        Texture2D ShipSprite { get; }

        Texture2D ShipExplosionSequence { get; }

        void MoveLeft();

        void MoveRight();

        void MoveUp();

        void MoveDown();

        void UpgradeWeapon();

        void DowngradeWeapon();

        void FireWeapon();
    }
}
