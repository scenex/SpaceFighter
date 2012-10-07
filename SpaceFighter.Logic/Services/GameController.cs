// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using Microsoft.Xna.Framework;

    public class GameController : GameComponent, IGameController
    {
        private readonly PlayerService playerService;

        private readonly EnemiesService enemyService;

        private readonly CollisionDetectionService collisionDetectionService;

        public GameController(Game game) : base(game)
        {
            this.collisionDetectionService = new CollisionDetectionService(game);
            this.Game.Components.Add(this.collisionDetectionService);

            this.playerService = new PlayerService(game);
            this.Game.Services.AddService(typeof(IPlayerService), this.playerService);
            this.Game.Components.Add(this.playerService);

            this.enemyService = new EnemiesService(game);
            this.Game.Services.AddService(typeof(IEnemiesService), this.enemyService);
            this.Game.Components.Add(this.enemyService);
        }

        public IPlayerService PlayerService
        {
            get
            {
                return this.playerService;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
