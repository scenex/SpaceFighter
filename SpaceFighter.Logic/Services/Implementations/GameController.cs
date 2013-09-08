// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : DrawableGameComponent, IGameController
    {
        private readonly Game game;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

        private double elapsedTime;

        private Curve fadeInCurve;
        private Curve fadeOutCurve;
        private readonly Dictionary<string, Curve> curves = new Dictionary<string, Curve>(); 

        private readonly ICollisionDetectionService collisionDetectionService;
        private readonly IPlayerService playerService;
        private readonly IEnemyService enemyService;
        private readonly IInputService inputService;
        private readonly IHeadUpDisplayService headUpDisplayService;
        private readonly ITerrainService terrainService;
        private readonly ICameraService cameraService;
        private readonly IAudioService audioService;
        private IDebugService debugService;

        private string currentState;

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

            this.currentState = "Starting";
        }



        public override void Initialize()
        {
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

            this.renderTarget = new RenderTarget2D(
                this.GraphicsDevice,
                this.game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                this.game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public string CurrentState
        {
            get
            {
                return this.currentState;
            }
            set
            {
                if(this.currentState != value)
                {
                    this.elapsedTime = 0;
                    this.currentState = value;
                }
            }
        }

        public void StartGame()
        {
            // DISABLE MUSIC WHILE DEVELOPMENT
            // this.audioService.PlaySound("music2");
            
            this.elapsedTime = 0;
            
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
            this.GraphicsDevice.SetRenderTarget(null);

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

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
            
            this.fadeInCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeIn");
            this.fadeOutCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeOut");

            this.curves.Add("Starting", this.fadeInCurve);
            this.curves.Add("Ending", this.fadeOutCurve);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            this.UpdatePlayerPositionForEnemies();
            
            base.Update(gameTime);

            this.game.GraphicsDevice.SetRenderTarget(renderTarget);
        }
        
        public override void Draw(GameTime gameTime)
        {
            foreach (var component in this.Game.Components)
            {
                var drawableComponent = component as IDrawable;
                if (drawableComponent != null)
                {
                    if(drawableComponent.Visible)
                    {
                        drawableComponent.Draw(gameTime);
                    }
                }
            }

            this.game.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(
                renderTarget, 
                renderTarget.Bounds, 
                this.currentState == "Starting" || this.currentState == "Ending" 
                    ? Color.White * this.curves[this.CurrentState].Evaluate((float)this.elapsedTime / 1000)
                    : Color.White * 1
                );
            
            spriteBatch.End();
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
