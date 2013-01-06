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
        private ICollisionDetectionService collisionDetectionService;
        private IPlayerService playerService;
        private IEnemyService enemyService;
        private IInputService inputService;

        public GameController(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.collisionDetectionService = (ICollisionDetectionService)this.Game.Services.GetService(typeof(ICollisionDetectionService));
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
            this.enemyService = (IEnemyService)this.Game.Services.GetService(typeof(IEnemyService));
            this.inputService = (IInputService)this.Game.Services.GetService(typeof(IInputService));

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            this.collisionDetectionService.PlayerEnemyHit += this.OnPlayerEnemyHit;
            this.collisionDetectionService.BoundaryHit += this.OnBoundaryHit;
            
            base.Initialize();
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.playerService.RemoveShot(e.Shot);
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs e)
        {
            this.enemyService.RemoveShot(e.Shot);
            this.playerService.ReportPlayerHit(e.Shot);

            this.playerService.SetStateDead();
            this.inputService.DisableInputDevice();
            this.collisionDetectionService.DisableCollisionDetection();
        }

        private void OnPlayerEnemyHit(object sender, EventArgs e)
        {
            this.playerService.SetStateDead();
            this.inputService.DisableInputDevice();
            this.collisionDetectionService.DisableCollisionDetection();
        }

        private void OnBoundaryHit(object sender, EventArgs e)
        {
            this.playerService.SetStateDead();
            this.inputService.DisableInputDevice();
            this.collisionDetectionService.DisableCollisionDetection();
        }
    }
}
