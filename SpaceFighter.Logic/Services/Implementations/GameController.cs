// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : DrawableGameComponent, IGameController
    {
        private ICollisionDetectionService collisionDetectionService;
        private IPlayerService playerService;
        private IEnemyService enemyService;
        private IInputService inputService;
        private IHeadUpDisplayService headUpDisplay;
        private ITerrainService worldService;
        private IDebugService debugService;

        public GameController(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.collisionDetectionService = (ICollisionDetectionService)this.Game.Services.GetService(typeof(ICollisionDetectionService));
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
            this.enemyService = (IEnemyService)this.Game.Services.GetService(typeof(IEnemyService));
            this.inputService = (IInputService)this.Game.Services.GetService(typeof(IInputService));
            this.headUpDisplay = (IHeadUpDisplayService)this.Game.Services.GetService(typeof(IHeadUpDisplayService));
            this.worldService = (ITerrainService)this.Game.Services.GetService(typeof(ITerrainService));
            this.debugService = (IDebugService)this.Game.Services.GetService(typeof(IDebugService));

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            this.collisionDetectionService.PlayerEnemyHit += this.OnPlayerEnemyHit;
            this.collisionDetectionService.BoundaryHit += this.OnBoundaryHit;

            this.playerService.TransitionToStateDying += this.OnTransitionToStateDying;
            this.playerService.TransitionToStateDead += this.OnTransitionToStateDead;
            this.playerService.TransitionToStateRespawn += this.OnTransitionToStateRespawn;
            this.playerService.TransitionToStateAlive += this.OnTransitionToStateAlive;
            this.playerService.HealthChanged += this.OnHealthChanged;
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdatePlayerPositionForEnemies();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.debugService.DrawRectangle(new Rectangle(((int)playerService.Player.Position.X / 80) * 80, ((int)playerService.Player.Position.Y / 80) * 80, 80, 80));
            base.Draw(gameTime);
        }

        private void UpdatePlayerPositionForEnemies()
        {
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                enemy.PlayerPosition = this.playerService.Player.Position;
            }
        }

        private void OnTransitionToStateDying(object sender, EventArgs eventArgs)
        {   
            this.inputService.Disable();
            this.collisionDetectionService.Disable();  
        }

        private void OnTransitionToStateDead(object sender, EventArgs eventArgs)
        {
            // Continue...
        }

        private void OnTransitionToStateRespawn(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.inputService.Enable();
        }

        private void OnTransitionToStateAlive(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.collisionDetectionService.Enable();
        }

        private void OnHealthChanged(object sender, HealthChangedEventArgs healthChangedEventArgs)
        {
            this.headUpDisplay.Health = healthChangedEventArgs.NewHealth;
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
            this.headUpDisplay.Health = this.playerService.Player.Health;
        }

        private void OnPlayerEnemyHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplay.Health = this.playerService.Player.Health;
        }

        private void OnBoundaryHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplay.Health = this.playerService.Player.Health;
        }
    }
}
