// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : GameComponent, IGameController
    {
        private GameStateEngine gameStateEngine;

        private readonly Game game;

        private readonly ICollisionDetectionService collisionDetectionService;
        private readonly IPlayerService playerService;
        private readonly IEnemyService enemyService;
        private readonly IInputService inputService;
        private readonly IHeadUpDisplayService headUpDisplayService;
        private readonly ITerrainService terrainService;
        private readonly ICameraService cameraService;
        private readonly IAudioService audioService;
        private IDebugService debugService;

        public GameController(
            Game game,
            ICollisionDetectionService collisionDetectionService,
            IPlayerService playerService,
            IEnemyService enemyService,
            IInputService inputService,
            IHeadUpDisplayService headUpDisplayService,
            ITerrainService terrainService,
            IDebugService debugService,
            IAudioService audioService,
            ICameraService cameraService) : base(game)
        {
            this.game = game;

            this.collisionDetectionService = collisionDetectionService;
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.inputService = inputService;
            this.headUpDisplayService = headUpDisplayService;
            this.terrainService = terrainService;
            this.debugService = debugService;
            this.audioService = audioService;
            this.cameraService = cameraService;
        }

        public override void Initialize()
        {
            this.gameStateEngine = new GameStateEngine(this.playerService, this.enemyService, this.inputService);

            this.game.Components.Add(new FramerateCounter(this.game));
            this.game.Components.Add(this.collisionDetectionService);
            this.game.Components.Add(this.playerService);
            this.game.Components.Add(this.enemyService);
            this.game.Components.Add(this.inputService);
            this.game.Components.Add(this.headUpDisplayService);
            this.game.Components.Add(this.terrainService);
            this.game.Components.Add(this.debugService);
            this.game.Components.Add(this.audioService);
            this.game.Components.Add(this.cameraService);

            base.Initialize();
        }

        public void StartGame()
        {
            // DISABLE MUSIC WHILE DEVELOPMENT
            // this.audioService.PlaySound("music2");

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            this.collisionDetectionService.PlayerEnemyHit += this.OnPlayerEnemyHit;
            this.collisionDetectionService.BoundaryHit += this.OnBoundaryHit;

            this.playerService.TransitionToStateDying += this.OnTransitionToStateDying;
            this.playerService.TransitionToStateDead += this.OnTransitionToStateDead;
            this.playerService.TransitionToStateRespawn += this.OnTransitionToStateRespawn;
            this.playerService.TransitionToStateAlive += this.OnTransitionToStateAlive;
            this.playerService.HealthChanged += this.OnHealthChanged;

            this.inputService.InputStateHandling = InputStateHandling.Gameplay;

            this.playerService.SpawnPlayer();
            this.enemyService.SpawnEnemies();

            this.inputService.Enable();
        }

        public void EndGame()
        {
            this.collisionDetectionService.EnemyHit -= this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit -= this.OnPlayerHit;
            this.collisionDetectionService.PlayerEnemyHit -= this.OnPlayerEnemyHit;
            this.collisionDetectionService.BoundaryHit -= this.OnBoundaryHit;

            this.playerService.TransitionToStateDying -= this.OnTransitionToStateDying;
            this.playerService.TransitionToStateDead -= this.OnTransitionToStateDead;
            this.playerService.TransitionToStateRespawn -= this.OnTransitionToStateRespawn;
            this.playerService.TransitionToStateAlive -= this.OnTransitionToStateAlive;
            this.playerService.HealthChanged -= this.OnHealthChanged;

            this.playerService.UnspawnPlayer();
            this.enemyService.UnspawnEnemies();
        }

        public void PauseGame()
        {
            foreach (var updateableComponent in this.game.Components.OfType<GameComponent>())
            {
                if (updateableComponent.GetType() != typeof(InputService) && updateableComponent.GetType() != typeof(GameController)) // Todo: Reflection alternative?
                {
                    updateableComponent.Enabled = false;
                }
            }
        }

        public void ResumeGame()
        {
            foreach (var updateableComponent in this.game.Components.OfType<GameComponent>())
            {
                if (updateableComponent.GetType() != typeof(InputService) && updateableComponent.GetType() != typeof(GameController)) // Todo: Reflection alternative?
                {
                    updateableComponent.Enabled = true;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdatePlayerPositionForEnemies();
            this.gameStateEngine.Update(gameTime);
            
            base.Update(gameTime);
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
            this.headUpDisplayService.Health = healthChangedEventArgs.NewHealth;
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
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }

        private void OnPlayerEnemyHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }

        private void OnBoundaryHit(object sender, EventArgs e)
        {
            this.playerService.ReportPlayerHit(100);
            this.headUpDisplayService.Health = this.playerService.Player.Health;
        }
    }
}
