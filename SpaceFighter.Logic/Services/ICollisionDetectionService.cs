// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System;

    public interface ICollisionDetectionService
    {
        // Todo: Specific EventArgs
        event EventHandler<EventArgs> CollisionDetected;
    }
}
