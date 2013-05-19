// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Behaviours.Interfaces;

    public abstract class BehaviourStrategy : IBehaviourStrategy
    {
        public abstract Vector2 Execute(Vector2 source, Vector2 target);
    }
}
