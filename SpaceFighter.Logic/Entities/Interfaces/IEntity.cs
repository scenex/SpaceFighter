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
        Vector2 Origin { get; } // <- rename to 'AbsoluteOrigin' ?
        int Width { get; }
        int Height { get; }
        int Health { get; }
        float Rotation { get; }
        Rectangle BoundingRectangle { get; }

        IWeapon Weapon { get; }
    }
}
