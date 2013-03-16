// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Enemies;

    public static class EnemyFactory
    {
        public static T Create<T>(Game game, Vector2 startPosition) where T : EnemyBase
        {
            return (T)Activator.CreateInstance(typeof(T), game, startPosition);
        }
    }
}
