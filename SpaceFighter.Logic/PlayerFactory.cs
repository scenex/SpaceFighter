// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Players;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerFactory : IPlayerFactory
    {
        private readonly Game game;

        private readonly ICameraService cameraService;

        public PlayerFactory(Game game, ICameraService cameraService)
        {
            this.game = game;
            this.cameraService = cameraService;
        }

        public PlayerA Create(Vector2 startPosition)
        {
            return new PlayerA(game, cameraService, startPosition);
        }
    }
}
