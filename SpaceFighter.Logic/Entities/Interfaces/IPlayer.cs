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
    public interface IPlayer : IEntity
    {
        event EventHandler<StateChangedEventArgs> ShipReady;
        event EventHandler<StateChangedEventArgs> ShipExploding;
        event EventHandler<StateChangedEventArgs> ShipRespawning;

        void AddHealth(int amount);
        void SubtractHealth(int amount); 
      
        void SetRotationDelta(float angleDelta);
        void SetRotation(float angle);

        new Vector2 Position { get; set; } // We need the setter

        IWeapon Weapon { get; }
        int Health { get; }
        int Lives { get; } 
    }
}
