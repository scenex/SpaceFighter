// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Enemies;

    public interface IEnemyFactory
    {
        T Create<T>(Vector2 startPosition) where T : EnemyBase;
    }
}