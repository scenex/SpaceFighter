// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    public static class EnemyFactory
    {
        public static T Create<T>(Game game, IEnumerable<Vector2> waypoints)
        {
            return (T)Activator.CreateInstance(typeof(T), game, waypoints);
        }
    }
}
