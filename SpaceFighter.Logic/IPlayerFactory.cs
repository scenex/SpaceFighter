// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Players;

    public interface IPlayerFactory
    {
        PlayerA Create(Vector2 startPosition);
    }
}