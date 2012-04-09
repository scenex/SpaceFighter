// -----------------------------------------------------------------------
// <copyright file="ISpaceship.cs" company="Cataclysm">
// TODO: Update copyright text.
// </copyright>
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

        Texture2D SpriteRegular { get; }

        Texture2D SpriteExplosionSequence { get; }

        IWeapon Weapon { get; }
    }
}
