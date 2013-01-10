// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    public interface IEnemy
    {
        Color[] ColorData { get; }
        Vector2 Position { get; }
        Vector2 Origin { get; }
        int Width { get; }
        int Height { get; }
        int Health { get; set; }
        Queue<TimeSpan> WeaponTriggers { get; }
        float Rotation { get; }

        IEnumerable<Vector2> Waypoints { get; } 

        double AngleToPlayer { get; }
        void UpdateAngleToPlayer(double angle);
    }
}
