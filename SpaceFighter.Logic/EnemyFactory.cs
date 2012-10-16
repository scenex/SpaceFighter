// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework;

    public static class EnemyFactory
    {
        public static T Create<T>(Game game, Vector2 startPosition)
        {
            return (T)Activator.CreateInstance(typeof(T), game, startPosition);
        }
    }
}
