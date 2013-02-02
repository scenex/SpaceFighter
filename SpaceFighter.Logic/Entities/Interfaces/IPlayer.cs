// -----------------------------------------------------------------------
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
        event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;
        event EventHandler<HealthChangedEventArgs> HealthChanged;
                 
        void SubtractHealth(int amount);
        void AddHealth(int amount);

        void SetRotation(float angle);   

        void Thrust(int amount);
    }
}
