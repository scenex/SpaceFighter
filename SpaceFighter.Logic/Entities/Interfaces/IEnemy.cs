// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IEnemy : IEntity
    {
        bool IsAlive { get; }
        void SubtractHealth(int amount);
        void AddHealth(int amount);

        // hmm... is there a better way?
        double AngleToPlayer { get; }
        void UpdateAngleToPlayer(double angle);
        void UpdateDistanceToPlayer(Vector2 distance);
    }
}
