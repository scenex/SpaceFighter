// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : GameComponent, IGameController
    {
        private readonly CollisionDetectionService collisionDetectionService;
        private readonly PlayerService playerService;
        private readonly EnemiesService enemyService;
        private readonly WeaponService weaponService;

        public GameController(Game game) : base(game)
        {
            // TODO: Move into Initialize() or LoadContent()
            this.collisionDetectionService = new CollisionDetectionService(game);
            this.Game.Components.Add(this.collisionDetectionService);

            this.playerService = new PlayerService(game);
            this.Game.Services.AddService(typeof(IPlayerService), this.playerService);
            this.Game.Components.Add(this.playerService);

            this.enemyService = new EnemiesService(game);
            this.Game.Services.AddService(typeof(IEnemiesService), this.enemyService);
            this.Game.Components.Add(this.enemyService);

            this.weaponService = new WeaponService(game);
            this.Game.Services.AddService(typeof(IWeaponService), this.weaponService);
            this.Game.Components.Add(this.weaponService);
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
