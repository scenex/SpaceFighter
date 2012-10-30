// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : GameComponent, IGameController
    {
        private CollisionDetectionService collisionDetectionService;
        private PlayerService playerService;
        private EnemyService enemyService;
        private PlayerWeaponService playerWeaponService;

        private EnemyWeaponService enemyWeaponService;

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

            this.playerWeaponService = new PlayerWeaponService(this.Game);
            this.Game.Services.AddService(typeof(IPlayerWeaponService), this.playerWeaponService);
            this.Game.Components.Add(this.playerWeaponService);

            this.enemyService = new EnemyService(this.Game);
            this.Game.Services.AddService(typeof(IEnemyService), this.enemyService);
            this.Game.Components.Add(this.enemyService);

            this.enemyWeaponService = new EnemyWeaponService(this.Game);
            this.Game.Services.AddService(typeof(IEnemyWeaponService), this.enemyWeaponService);
            this.Game.Components.Add(this.enemyWeaponService);

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            
            base.Initialize();
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs e)
        {
            this.playerService.ReportPlayerHit(e.Shot);

            // Todo: Remove shot
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);
            this.playerWeaponService.RemoveShot(e.Shot);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
