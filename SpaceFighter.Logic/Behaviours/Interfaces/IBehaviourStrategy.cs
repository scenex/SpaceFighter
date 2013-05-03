// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IBehaviourStrategy
    {
        Vector2 Execute(Vector2 source, Vector2 target);
    }
}
