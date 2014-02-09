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
    using SpaceFighter.Logic.StateMachine;

    public class GameController : DrawableGameComponent, IGameController
    {
        private const double FadeEffectDuration = 1500;

        private readonly Game game;
        private SpriteBatch spriteBatch;
        private RenderTarget2D gameRenderTarget;

        private Curve fadeInCurve;
        private Curve fadeOutCurve;
        private readonly Dictionary<string, Curve> curves = new Dictionary<string, Curve>();

        private readonly ICollisionDetectionService collisionDetectionService;
        private readonly IPlayerService playerService;
        private readonly IEnemyService enemyService;
        private readonly IInputService inputService;
        private readonly IHeadUpDisplayService headUpDisplayService;
        private readonly ITerrainService terrainService;
        private readonly IAudioService audioService;

        private StateMachine<Action<double>> gameStateMachine;

        private string fadeEffect;
        private double fadeEffectElapsed;

        private double elapsedTime;
        private double elapsedTimeSinceEndingTransition;

        private bool isGamePaused;

        public GameController(
            Game game,
            ICollisionDetectionService collisionDetectionService,
            IPlayerService playerService,
            IEnemyService enemyService,
            IInputService inputService,
            IHeadUpDisplayService headUpDisplayService,
            ITerrainService terrainService,
            IAudioService audioService) : base(game)
        {
            this.game = game;

            this.collisionDetectionService = collisionDetectionService;
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.inputService = inputService;
            this.headUpDisplayService = headUpDisplayService;
            this.terrainService = terrainService;
            this.audioService = audioService;

            this.inputService.AnalogPauseChanged += delegate { this.isGamePaused = !this.isGamePaused; };
            this.inputService.PauseChanged += delegate { this.isGamePaused = !this.isGamePaused; };

            this.fadeEffect = "FadeIn";
        }

        public bool IsGameRunning { get; private set; }

        public override void Initialize()
        {
            this.SetupStateEngine();

            base.Initialize();

            this.gameRenderTarget = new RenderTarget2D(
                this.GraphicsDevice,
                this.game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                this.game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);

            this.fadeInCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeIn");
            this.fadeOutCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeOut");

            this.curves.Add("FadeIn", this.fadeInCurve);
            this.curves.Add("FadeOut", this.fadeOutCurve);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.gameStateMachine.Update();

            this.headUpDisplayService.Health = this.playerService.Player.Health;
            this.headUpDisplayService.Lives = this.playerService.Player.Lives;

            if (this.IsGameRunning)
            {
                this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (this.fadeEffect == "FadeIn" || this.fadeEffect == "FadeOut")
                {
                    this.fadeEffectElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                this.UpdatePlayerPositionForEnemies();
                base.Update(gameTime);
                this.game.GraphicsDevice.SetRenderTarget(this.gameRenderTarget);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.IsGameRunning)
            {
                this.game.GraphicsDevice.SetRenderTarget(null);
                this.GraphicsDevice.Clear(Color.Black);

                this.spriteBatch.Begin(
                    SpriteSortMode.BackToFront,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    null,
                    Matrix.CreateTranslation(2*80, 0, 1)); // Global translation

                spriteBatch.Draw(
                    this.gameRenderTarget,
                    this.gameRenderTarget.Bounds, 
                    this.fadeEffect == "FadeIn" || this.fadeEffect == "FadeOut"
                        ? Color.White * this.curves[this.fadeEffect].Evaluate((float)(this.fadeEffectElapsed) / 1000)
                        : Color.White * 1);
                
                spriteBatch.End();

                this.headUpDisplayService.Draw(gameTime,
                    this.fadeEffect == "FadeIn" || this.fadeEffect == "FadeOut"
                        ? Color.White * this.curves[this.fadeEffect].Evaluate((float)(this.fadeEffectElapsed) / 1000)
                        : Color.White * 1);

                //base.Draw(gameTime); // Todo: Needed?
            }
        }

        public void StartGame()
        {
            // DISABLE MUSIC WHILE DEVELOPMENT
            // this.audioService.PlaySound("music2");

            this.IsGameRunning = true;

            this.game.Components.Add(this.collisionDetectionService);
            this.game.Components.Add(this.playerService);
            this.game.Components.Add(this.enemyService);
            this.game.Components.Add(this.inputService);
            this.game.Components.Add(this.headUpDisplayService);
            this.game.Components.Add(this.terrainService);
            this.game.Components.Add(this.audioService);

            this.playerService.ShipReady += this.OnShipReady;
            this.playerService.ShipExploding += this.OnShipExploding;
            this.playerService.ShipRespawning += this.OnShipRespawning;

            this.playerService.SpawnPlayer();
            this.enemyService.SpawnEnemies();
        }

        private void PauseGame()
        {
            foreach (var updateableComponent in this.game.Components.OfType<GameComponent>())
            {
                if (updateableComponent.GetType() != typeof(InputService) && updateableComponent.GetType() != typeof(GameController)) // Todo: Reflection alternative?
                {
                    updateableComponent.Enabled = false;
                }
            }
        }

        private void ResumeGame()
        {
            foreach (var updateableComponent in this.game.Components.OfType<GameComponent>())
            {
                if (updateableComponent.GetType() != typeof(InputService) && updateableComponent.GetType() != typeof(GameController)) // Todo: Reflection alternative?
                {
                    updateableComponent.Enabled = true;
                }
            }
        }

        private void EndGame()
        {
            this.IsGameRunning = false;

            this.GraphicsDevice.SetRenderTarget(null);

            this.playerService.ShipReady -= this.OnShipReady;
            this.playerService.ShipExploding -= this.OnShipExploding;
            this.playerService.ShipRespawning -= this.OnShipRespawning;

            this.playerService.UnspawnPlayer();
            this.enemyService.UnspawnEnemies();

            this.game.Components.Remove(this.collisionDetectionService);
            this.game.Components.Remove(this.playerService);
            this.game.Components.Remove(this.enemyService);
            this.game.Components.Remove(this.inputService);
            this.game.Components.Remove(this.headUpDisplayService);
            this.game.Components.Remove(this.terrainService);
            this.game.Components.Remove(this.audioService);
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

        private void SetupStateEngine()
        {
            var idle = new State<Action<double>>(
                "Idle",
                null,
                null,
                null);

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

                    // Todo: Restarting only for development purposes, otherwise next level or game finished.
                    this.EndGame();
                    this.StartGame();
                },
                null);

            var paused = new State<Action<double>>(
                "Paused",
                null,
                this.PauseGame,
                this.ResumeGame);

            var gameOver = new State<Action<double>>(
                "GameOver",
                null,
                this.EndGame,
                null);

            idle.AddTransition(
                starting,
                () => this.IsGameRunning);

            starting.AddTransition(
                started,
                () => this.elapsedTime > FadeEffectDuration);

            started.AddTransition(
                ending,
                () => this.enemyService.IsBossEliminated || this.playerService.Player.Lives <= 0);

            started.AddTransition(
                paused,
                () => this.isGamePaused == true);

            paused.AddTransition(
                started,
                () => this.isGamePaused == false);

            ending.AddTransition(
                ended,
                () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > FadeEffectDuration && this.enemyService.IsBossEliminated);

            ending.AddTransition(
                gameOver,
                () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > FadeEffectDuration && this.playerService.Player.Lives == 0);

            ended.AddTransition(starting, () => true);
            gameOver.AddTransition(idle, () => true);

            this.gameStateMachine = new StateMachine<Action<double>>(idle);
        }

        // Todo: Somewhat ugly..
        private void UpdatePlayerPositionForEnemies()
        {
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                enemy.PlayerPosition = this.playerService.Player.Position;
            }
        }

        private void OnShipReady(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.collisionDetectionService.Enable();
        }

        private void OnShipExploding(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.collisionDetectionService.Disable();
        }

        private void OnShipRespawning(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.collisionDetectionService.Disable();
        }
    }
}