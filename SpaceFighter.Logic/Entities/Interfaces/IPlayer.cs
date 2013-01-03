// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The interface to represent the player's spaceship
    /// </summary>
    public interface IPlayer
    {
        Vector2 Position { get; set; }
        int Width { get; }
        int Height { get; }
        Color[] ColorData { get; }
        Vector2 Origin { get; }
        float Rotation { get; set; }
    }
}
