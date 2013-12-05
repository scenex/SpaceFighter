// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IEntity
    {
        Color[] ColorData { get; }
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }
        float Rotation { get; }
        Rectangle BoundingRectangle { get; }
    }
}
