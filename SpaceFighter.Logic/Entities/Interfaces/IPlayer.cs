// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The interface to represent the player's spaceship
    /// </summary>
    public interface IPlayer
    {
        event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;

        Vector2 Position { get; set; }
        int Width { get; }
        int Height { get; }
        Color[] ColorData { get; }
        Vector2 Origin { get; }
        float Rotation { get; set; }

        int Health { get; }
        void SubtractHealth(int amount);
        void AddHealth(int amount);

        void Thrust(int amount);
    }
}
