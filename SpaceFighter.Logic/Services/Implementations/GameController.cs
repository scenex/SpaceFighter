// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : GameComponent, IGameController
    {
        private CollisionDetectionService collisionDetectionService;
        private PlayerService playerService;
        private EnemiesService enemyService;
        private WeaponService weaponService;

        public GameController(Game game) : base(game)
        {
        }

        public IPlayerService PlayerService
        {
            get
            {
                return this.playerService;
            }
        }

        public override void Initialize()
        {
            this.collisionDetectionService = new CollisionDetectionService(this.Game);
            this.Game.Components.Add(this.collisionDetectionService);

            this.playerService = new PlayerService(this.Game);
            this.Game.Services.AddService(typeof(IPlayerService), this.playerService);
            this.Game.Components.Add(this.playerService);

            this.enemyService = new EnemiesService(this.Game);
            this.Game.Services.AddService(typeof(IEnemiesService), this.enemyService);
            this.Game.Components.Add(this.enemyService);

            this.weaponService = new WeaponService(this.Game);
            this.Game.Services.AddService(typeof(IWeaponService), this.weaponService);
            this.Game.Components.Add(this.weaponService);

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;

            base.Initialize();
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
