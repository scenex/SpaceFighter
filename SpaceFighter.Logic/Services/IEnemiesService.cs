// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System.Collections.Generic;

    public interface IEnemiesService
    {
        IEnumerable<IEnemy> Enemies { get; }
    }
}
