﻿// -----------------------------------------------------------------------
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
        event EventHandler<StateChangedEventArgs> ShipReady;
        event EventHandler<StateChangedEventArgs> ShipExploding;
        event EventHandler<StateChangedEventArgs> ShipRespawning;

        IPlayer Player { get; }
        IEnumerable<IShot> Shots { get; } 
        void ReportPlayerHit(IShot shot);
        void ReportPlayerHit(int damage);

        void RemoveShot(IShot shot);

        void Fire();
        void MoveUp();
        void MoveDown();
        void MoveLeft();
        void MoveRight();

        void Thrust(float angleDelta);

        void SpawnPlayer();
        void UnspawnPlayer();
    }
}
