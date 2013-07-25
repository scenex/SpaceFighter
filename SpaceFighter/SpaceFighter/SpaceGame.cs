// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;

    using Microsoft.Xna.Framework;

    using SpaceFighter.GameStates;
    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.StateMachine;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game
    {
        IPlayerFactory playerFactory;
        IEnemyFactory enemyFactory;

        ITerrainService terrainService;
        IHeadUpDisplayService headUpDisplayService;
        IAudioService audioService;
        IPlayerService playerService;
        IInputService inputService;
        IEnemyService enemyService;
        ICollisionDetectionService collisionDetectionService;
        ICameraService cameraService;
        IDebugService debugService;
        IGameController gameController;

        private readonly GraphicsDeviceManager graphics;

        private readonly GameStateManager gameStateManager = new GameStateManager();

        private StateMachine<Action<double>> applicationStateMachine;

        private double elapsedTime;

        //SpriteBatch spriteBatch;
        //private Effect shader;
        //private float elapsed;
        //private RenderTarget2D renderTarget;

        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceGame"/> class.
        /// </summary>
        public SpaceGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {   
            this.graphics.PreferredBackBufferWidth = ScreenWidth;
            this.graphics.PreferredBackBufferHeight = ScreenHeight;
            this.IsMouseVisible = true;         
            //this.renderTarget = new RenderTarget2D(this.GraphicsDevice, ScreenWidth, ScreenHeight);
            this.graphics.ApplyChanges();

            this.ComposeServices();

            // Setup application state machine
            var intro = new State<Action<double>>(
                "Intro", 
                null,
                null, 
                () => this.gameStateManager.Pop());

            var menu = new State<Action<double>>(
                "Menu",
                null,
                () => this.gameStateManager.Push(new MenuGameState(this)),
                () => this.gameStateManager.Pop());

            var gameplay = new State<Action<double>>(
                "Gameplay",
                null,
                () => this.gameStateManager.Push(new GameplayGameState(this)),
                () => this.gameStateManager.Pop());

            intro.AddTransition(menu, () => this.elapsedTime > 4000);
            menu.AddTransition(gameplay, () => this.elapsedTime > 8000);
            
            this.applicationStateMachine = new StateMachine<Action<double>>(intro);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //this.spriteBatch = new SpriteBatch(GraphicsDevice);
            //this.shader = this.Content.Load<Effect>("Shaders/Circle");
        }

        private void ComposeServices()
        {            
            var translation = new Vector3(
                (this.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (80 / 2) + 62,  // (this.playerService.Player.Width / 2) + 62
                (this.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - (80 / 2) + 32, // (this.playerService.Player.Height / 2) + 32
                0); 

            cameraService = new CameraService(this, translation);
            this.Services.AddService(typeof(ICameraService), cameraService);

            terrainService = new TerrainService();
            this.Services.AddService(typeof(ITerrainService), terrainService);

            headUpDisplayService = new HeadUpDisplayService(this);
            this.Services.AddService(typeof(IHeadUpDisplayService), headUpDisplayService);

            audioService = new AudioService(this);
            this.Services.AddService(typeof(IAudioService), audioService);

            enemyFactory = new EnemyFactory(this, cameraService, terrainService);
            enemyService = new EnemyService(this, enemyFactory);
            this.Services.AddService(typeof(IEnemyService), enemyService);

            playerFactory = new PlayerFactory(this, this.cameraService);
            playerService = new PlayerService(this, audioService, playerFactory);
            this.Services.AddService(typeof(IPlayerService), playerService);

            inputService = new InputService(this, playerService);
            this.Services.AddService(typeof(IInputService), inputService);

            collisionDetectionService = new CollisionDetectionService(this, playerService, enemyService, terrainService);
            this.Services.AddService(typeof(ICollisionDetectionService), collisionDetectionService);

            debugService = new DebugService(this, cameraService);
            this.Services.AddService(typeof(IDebugService), debugService);

            gameController = new GameController(this, collisionDetectionService, playerService, enemyService, inputService, headUpDisplayService, terrainService, debugService, audioService, cameraService);
            this.Services.AddService(typeof(IGameController), gameController);
        }

        protected override void Update(GameTime gameTime)
        {
            if (this.gameStateManager.ActiveState == null)
            {
                this.gameStateManager.Push(new IntroGameState(this));
            }

            elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateManager.Update(gameTime);
            this.applicationStateMachine.Update();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);
            this.gameStateManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        #region Post Processing Shader
        
        //protected override void Update(GameTime gameTime)
        //{
        //    //this.elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        //    //this.shader.Parameters["circleRadius"].SetValue(this.elapsed);

        //    //gameStateManager.Push(new GameplayGameState()); <= Hook in here
        //    base.Update(gameTime);
        //}

        ///// <summary>
        ///// This is called when the game should draw itself.
        ///// </summary>
        ///// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override void Draw(GameTime gameTime)
        //{
        //    // Render to rendertarget
        //    //this.graphics.GraphicsDevice.SetRenderTarget(this.renderTarget);          
        //    //GraphicsDevice.Clear(Color.Black);
        //    base.Draw(gameTime);
        //    //this.graphics.GraphicsDevice.SetRenderTarget(null);

        //    // Render rendertarget to backbuffer
        //    //spriteBatch.Begin(
        //    //    SpriteSortMode.BackToFront,
        //    //    BlendState.AlphaBlend,
        //    //    null,
        //    //    null,
        //    //    null,
        //    //    //this.shader);
        //    //    null);
        //    //spriteBatch.Draw(this.renderTarget, this.renderTarget.Bounds, Color.White);            
        //    //spriteBatch.End();
        //}

        #endregion
    }
}
