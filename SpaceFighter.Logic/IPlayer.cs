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
    public interface IPlayer
    {
        Vector2 Position { get; set; }

        Texture2D Sprite { get; }

        Texture2D ExplosionSequence { get; }

        Color[] SpriteDataCached { get; }
    }
}
