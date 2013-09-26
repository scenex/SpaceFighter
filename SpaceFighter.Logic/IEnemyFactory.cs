// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Enemies;

    public interface IEnemyFactory
    {
        T CreateAutonomous<T>(Vector2 startPosition, bool isBoss) where T : EnemyAutonomous;
        T CreateScripted<T>(Vector2 startPosition, bool isBoss) where T : EnemyScripted;
    }
}