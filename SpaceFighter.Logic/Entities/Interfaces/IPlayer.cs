﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;

    /// <summary>
    /// The interface to represent the player's spaceship
    /// </summary>
    public interface IPlayer : IEntity
    {
        event EventHandler<StateChangedEventArgs> ShipVulnerable;
        event EventHandler<StateChangedEventArgs> ShipExploding;
        event EventHandler<StateChangedEventArgs> ShipInvincible;

        void Thrust();
        void AddHealth(int amount);
        void SubtractHealth(int amount);       
        void SetRotation(float angle);    
    }
}
