// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Players;

    public class PlayerFactory : IPlayerFactory
    {
        private readonly Game game;

        public PlayerFactory(Game game)
        {
            this.game = game;
        }

        public PlayerA Create(Vector2 startPosition)
        {
            return new PlayerA(game, startPosition);
        }
    }
}
