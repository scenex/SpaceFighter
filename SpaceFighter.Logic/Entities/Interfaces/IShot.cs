// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System;

    using Microsoft.Xna.Framework;

    public interface IShot
    {
        Guid Id { get; }
        Color[] ColorData { get; }
        Vector2 Position { get; set; }
        int Width { get; }
        int Height { get; }
        int FirePower { get; }
    }
}
