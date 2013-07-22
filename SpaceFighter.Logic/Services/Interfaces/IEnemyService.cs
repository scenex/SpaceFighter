// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IEnemyService : IGameComponent
    {
        IEnumerable<IEnemy> Enemies { get; }
        IEnumerable<IShot> Shots { get; } 
        void ReportEnemyHit(IEnemy enemy, IShot shot);
        void RemoveShot(IShot shot);

        void SpawnEnemies();
    }
}
