// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Behaviours.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public abstract class BehaviourStrategy : IBehaviourStrategy
    {
        protected IWorldService WorldService { get; private set; }

        protected BehaviourStrategy(IWorldService worldService)
        {
            this.WorldService = worldService;
        }

        public abstract Vector2 Execute(Vector2 source, Vector2 target);
    }
}
