// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IEntity
    {
        IWeapon Weapon { get; }

        Color[] ColorData { get; }
        Rectangle BoundingRectangle { get; }

        Vector2 Position { get; }
        float Rotation { get; }
        int Width { get; }
        int Height { get; }

        int Health { get; }
        int Lives { get; }        
    }
}
