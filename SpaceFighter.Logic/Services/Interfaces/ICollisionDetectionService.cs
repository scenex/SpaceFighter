// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    using Microsoft.Xna.Framework;

    public interface ICollisionDetectionService : IGameComponent
    {
        event EventHandler<EventArgs> PlayerEnemyHit;
        event EventHandler<EnemyHitEventArgs> EnemyHit;
        event EventHandler<PlayerHitEventArgs> PlayerHit;
        event EventHandler<EventArgs> BoundaryHit;

        void Enable();
        void Disable();
    }
}
