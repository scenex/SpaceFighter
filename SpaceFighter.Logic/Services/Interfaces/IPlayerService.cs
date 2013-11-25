// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IPlayerService : IGameComponent
    {
        event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;

        IPlayer Player { get; }
        IEnumerable<IShot> Shots { get; } 
        void ReportPlayerHit(IShot shot);
        void ReportPlayerHit(int damage);

        void RemoveShot(IShot shot);

        void RotateLeft();
        void RotateRight();
        void Fire();
        void Thrust();

        void SpawnPlayer();
        void UnspawnPlayer();
    }
}
