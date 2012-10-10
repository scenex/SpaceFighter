// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System;

    public interface ICollisionDetectionService
    {
        // Todo: Specific EventArgs
        event EventHandler<EventArgs> PlayerEnemyCollisionDetected;

        event EventHandler<EventArgs> EnemyHitCollisionDetected;

        event EventHandler<EventArgs> PlayerHitCollisionDetected;
    }
}
