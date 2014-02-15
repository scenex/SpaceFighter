// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    public interface IEnemy : IEntity
    {
        Vector2 PlayerPosition { get; set; }
        List<Vector2> Waypoints { get; }

        void SubtractHealth(int amount);
        void AddHealth(int amount);

        bool IsHealthSubtracted { get; }
        bool IsHealthAdded { get; }

        bool IsBoss { get; }

        IWeapon Weapon { get; }
        int Health { get; }
    }
}
