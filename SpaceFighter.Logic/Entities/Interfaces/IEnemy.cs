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
        int Width { get; }
        int Height { get; }
        int Energy { get; set; }
        Queue<TimeSpan> WeaponTriggers { get; }
        float Rotation { get; }
    }
}
