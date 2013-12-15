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

        void Thrust();
        void AddHealth(int amount);
        void SubtractHealth(int amount);       
        void SetRotation(float angleDelta);

        void Move(Vector2 moveBy);

        IWeapon Weapon { get; }
        int Health { get; }
        int Lives { get; } 
    }
}
