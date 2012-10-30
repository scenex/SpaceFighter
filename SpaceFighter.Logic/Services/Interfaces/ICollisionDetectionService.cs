// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    public interface ICollisionDetectionService
    {
        event EventHandler<EventArgs> PlayerEnemyHit;
        event EventHandler<EnemyHitEventArgs> EnemyHit;
        event EventHandler<PlayerHitEventArgs> PlayerHit;
    }
}
