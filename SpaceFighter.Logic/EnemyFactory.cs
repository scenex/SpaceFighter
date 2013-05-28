// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Enemies;
    using SpaceFighter.Logic.Services.Interfaces;

    public static class EnemyFactory
    {
        public static T Create<T>(Game game, IPathFindingService pathFindingService, Vector2 startPosition) where T : EnemyBase
        {
            return (T)Activator.CreateInstance(typeof(T), game, pathFindingService, startPosition);
        }
    }
}
