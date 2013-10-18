// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Enemies;

    public interface IEnemyFactory
    {
        T CreateAutonomous<T>(Vector2 startPosition, bool isBoss) where T : EnemyAutonomous;
        T CreateScripted<T>(Queue<Vector2> waypoints, bool isBoss) where T : EnemyScripted;
    }
}