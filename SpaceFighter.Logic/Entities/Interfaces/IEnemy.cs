// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    public interface IEnemy : IEntity
    {
        bool IsAlive { get; }
        void SubtractHealth(int amount);
        void AddHealth(int amount);

        Queue<TimeSpan> WeaponTriggers { get; }
        IEnumerable<Vector2> Waypoints { get; } 

        // hmm... is there a better way?
        double AngleToPlayer { get; }
        void UpdateAngleToPlayer(double angle);
    }
}
