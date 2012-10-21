// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IEnemyService
    {
        IEnumerable<IEnemy> Enemies { get; }
        IEnumerable<IShot> Shots { get; } 
        void ReportEnemyHit(IEnemy enemy, IShot shot);
    }
}
