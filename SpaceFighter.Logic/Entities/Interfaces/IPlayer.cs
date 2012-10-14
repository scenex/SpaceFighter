// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The interface to represent the player's spaceship
    /// </summary>
    public interface IPlayer
    {
        Vector2 Position { get; set; }
        Texture2D Sprite { get; }
        Color[] SpriteDataCached { get; }
        Texture2D ExplosionSequence { get; }
    }
}
