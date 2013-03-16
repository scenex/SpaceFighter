// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ISteeringStrategy
    {
        Vector2 Execute(Vector2 position, Vector2 distance, float angle);
    }
}
