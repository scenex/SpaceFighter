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

    using SpaceFighter.Logic.EventManager;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class GameController : DrawableGameComponent, IGameController
    {
        private const double FadeEffectDuration = 1500;

        private readonly Game game;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;

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
        private SpriteFont font;

        private StateMachine<Action<double>> gameStateMachine;

        private string fadeEffect;
        private double fadeEffectElapsed;

        private double elapsedTime;
        private double elapsedTimeSinceEndingTransition;

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

            this.fadeEffect = string.Empty;

            // ----- REWRITE 2013 -----
            this.SetupStateEngine();
        }

        private void SetupStateEngine()
        {
            var starting = new State<Action<double>>(
                "Starting",
                null,
                this.FadeIn,
                null);

            var started = new State<Action<double>>(
                "Started",
                null,
                null,
                null);

            var ending = new State<Action<double>>(
                "Ending",
                null,
                delegate
                {
                    this.elapsedTimeSinceEndingTransition = this.elapsedTime;
                    this.FadeOut();
                },
                null);

            var ended = new State<Action<double>>(
                "Ended",
                null,
                delegate
                {
                    this.elapsedTime = 0;
                    this.elapsedTimeSinceEndingTransition = 0;

                    this.EndGame();
                    this.StartGame();
                },
                null);

            var paused = new State<Action<double>>(
                "Paused",
                null,
                () => EventAggregator.Fire(this, "PauseToggled"),
                () => EventAggregator.Fire(this, "PauseToggled"));

            var gameOver = new State<Action<double>>(
                "GameOver",
                null,
                null,
                null
                /*() => this.reset = false*/);

            starting.AddTransition(
                started, 
                () => this.elapsedTime > FadeEffectDuration);

            started.AddTransition(
                ending, 
                () => this.enemyService.IsBossEliminated || this.playerService.Player.Health <= 0);

            started.AddTransition(
                paused, 
                () => this.inputService.IsGamePaused == true);

            paused.AddTransition(
                started, 
                () => this.inputService.IsGamePaused == false);

            ending.AddTransition(
                ended, 
                () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > FadeEffectDuration && this.enemyService.IsBossEliminated);
            
            ending.AddTransition(
                gameOver, 
                () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > FadeEffectDuration && this.playerService.Player.Health <= 0);
            
            //ended.AddTransition(starting, () => true);
            //gameOver.AddTransition(starting, () => this.reset);

            this.gameStateMachine = new StateMachine<Action<double>>(starting);
        }

        private void FadeIn()
        {
            this.fadeEffect = "FadeIn";
            this.fadeEffectElapsed = 0;
        }

        private void FadeOut()
        {
            this.fadeEffect = "FadeOut";
            this.fadeEffectElapsed = 0;
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

            this.curves.Add("FadeIn", this.fadeInCurve);
            this.curves.Add("FadeOut", this.fadeOutCurve);

            this.font = this.game.Content.Load<SpriteFont>(@"DefaultFont");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateMachine.Update();

            if(this.fadeEffect == "FadeIn" || this.fadeEffect == "FadeOut")
            {
                this.fadeEffectElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            }

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
                this.fadeEffect == "FadeIn" || this.fadeEffect == "FadeOut"
                    ? Color.White * this.curves[this.fadeEffect].Evaluate((float)(this.fadeEffectElapsed) / 1000)
                    : Color.White * 1);

            //spriteBatch.DrawString(this.font, Math.Round(elapsedTime / 1000, 1).ToString(), new Vector2(50, 20), Color.White);
            
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
