// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IPlayerService
    {
        event EventHandler<EventArgs> TransitionToStateAlive;
        event EventHandler<EventArgs> TransitionToStateDying;
        event EventHandler<EventArgs> TransitionToStateDead;
        event EventHandler<EventArgs> TransitionToStateRespawn;

        IPlayer Player { get; }
        IEnumerable<IShot> Shots { get; } 
        void ReportPlayerHit(IShot shot);
        void ReportPlayerHit(int damage);

        void RemoveShot(IShot shot);

        void RotateLeft();
        void RotateRight();
        void Fire();
        void Thrust();
    }
}
