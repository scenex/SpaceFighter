// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IEnemiesService
    {
        IEnumerable<IEnemy> Enemies { get; }
        void ReportEnemyHit(IEnemy enemy, IWeapon weapon);
    }
}
