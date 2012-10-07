// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using Microsoft.Xna.Framework;

    public class GameController : GameComponent, IGameController
    {
        private IPlayerService playerService;

        private IEnemiesService enemyService;

        private ICollisionDetectionService collisionDetectionService;

        public GameController(Game game) : base(game)
        {
            this.Game.Services.AddService(typeof(IPlayerService), new PlayerService(game));
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));

            this.Game.Services.AddService(typeof(IEnemiesService), new EnemiesService(game));
            this.enemyService = (IEnemiesService)this.Game.Services.GetService(typeof(IEnemiesService));

            this.Game.Services.AddService(typeof(ICollisionDetectionService), new CollisionDetectionService(game));
            this.collisionDetectionService = (ICollisionDetectionService)this.Game.Services.GetService(typeof(ICollisionDetectionService));
        }

        public IPlayerService PlayerService
        {
            get
            {
                return this.playerService;
            }
        }
    }
}
