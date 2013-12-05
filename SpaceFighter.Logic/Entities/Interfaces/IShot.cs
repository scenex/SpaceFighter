// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IShot : IEntity
    {
        int FirePower { get; }
        new Vector2 Position { get; set; } // We need the setter
    }
}