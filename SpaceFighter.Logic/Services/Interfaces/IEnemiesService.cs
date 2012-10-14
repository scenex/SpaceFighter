// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IEnemiesService
    {
        IEnumerable<IEnemy> Enemies { get; }
        void ReportEnemyHit(IEnemy enemy, IShot shot);
    }
}
