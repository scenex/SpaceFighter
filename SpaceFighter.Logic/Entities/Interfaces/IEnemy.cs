// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IEnemy : IEntity
    {
        Vector2 PlayerPosition { get; set; }

        void SubtractHealth(int amount);
        void AddHealth(int amount);

        bool IsHealthSubtracted { get; }
        bool IsHealthAdded { get; }
    }
}
