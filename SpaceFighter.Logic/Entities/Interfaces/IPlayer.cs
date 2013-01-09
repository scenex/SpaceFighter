// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations;

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

        PlayerState State { get; } 
        void RestartLifeCycle(bool respawn);

        void Thrust(int amount);
    }
}
