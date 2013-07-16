// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Input.Implementation;
    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game, ISpaceGame
    {
        private GraphicsDeviceManager graphics;
        private GameServiceContainer gameServiceContainer = new GameServiceContainer();

        private readonly GraphicsDeviceManager graphicsDeviceManager;

        private readonly IGameController gameController;
        private readonly IInputService inputService;
        private readonly ICollisionDetectionService collisionDetectionService;
        private readonly IPlayerService playerService;
        private readonly IEnemyService enemyService;
        private readonly ITerrainService terrainService;
        private readonly ICameraService cameraService;
        private readonly IHeadUpDisplayService headUpDisplayService;
        private readonly IAudioService audioService;
        private readonly IDebugService debugService;

        SpriteBatch spriteBatch;
        private Effect shader;

        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;
        private RenderTarget2D renderTarget;

        private float elapsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceGame"/> class.
        /// </summary>
        public SpaceGame(
            //GraphicsDeviceManager graphicsDeviceManager,
            IGameController gameController,
            IInputService inputService,
            ICollisionDetectionService collisionDetectionService,
            IPlayerService playerService,
            IEnemyService enemyService,
            ITerrainService terrainService,
            ICameraService cameraService,
            IHeadUpDisplayService headUpDisplayService,
            IAudioService audioService,
            IDebugService debugService)
        {
            //this.graphicsDeviceManager = graphicsDeviceManager;
            this.gameController = gameController;
            this.inputService = inputService;
            this.collisionDetectionService = collisionDetectionService;
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.terrainService = terrainService;
            this.cameraService = cameraService;
            this.headUpDisplayService = headUpDisplayService;
            this.audioService = audioService;
            this.debugService = debugService;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.graphics = new GraphicsDeviceManager(this);
            
            //this.graphics = graphicsDeviceManager;
            this.Content.RootDirectory = "Content";

            this.Services.AddService(typeof(IGameController), gameController);
            Components.Add(gameController);

            this.Services.AddService(typeof(IInputService), inputService);
            this.Components.Add(inputService);

            this.Services.AddService(typeof(ICollisionDetectionService), collisionDetectionService);
            this.Components.Add(collisionDetectionService);

            this.Services.AddService(typeof(IPlayerService), playerService);
            this.Components.Add(playerService);

            this.Services.AddService(typeof(IEnemyService), enemyService);
            this.Components.Add(enemyService);

            this.Services.AddService(typeof(ITerrainService), terrainService);
            this.Components.Add(terrainService);

            this.Services.AddService(typeof(ICameraService), cameraService);
            this.Components.Add(cameraService);

            this.Services.AddService(typeof(IHeadUpDisplayService), headUpDisplayService);
            this.Components.Add(headUpDisplayService);

            this.Services.AddService(typeof(IAudioService), audioService);
            this.Components.Add(audioService);

            this.Services.AddService(typeof(IDebugService), debugService);
            this.Components.Add(debugService);


            this.graphics.PreferredBackBufferWidth = ScreenWidth;
            this.graphics.PreferredBackBufferHeight = ScreenHeight;
            this.IsMouseVisible = true; 
            
            this.graphics.ApplyChanges();
            this.renderTarget = new RenderTarget2D(this.GraphicsDevice, ScreenWidth, ScreenHeight);

            this.RegisterGameServices();

            #if WINDOWS           
            if(((IInputService)(this.Services.GetService(typeof(IInputService)))).IsGamePadConnected)
            {
                ((IInputService)(this.Services.GetService(typeof(IInputService)))).SetInputDevice(new InputGamepad());
            }
            else
            {
                ((IInputService)(this.Services.GetService(typeof(IInputService)))).SetInputDevice(new InputKeyboard());
            }            
            #elif XBOX
            ((IInputService)(this.Services.GetService(typeof(IInputService)))).SetInputDevice(new InputGamepad());
            #endif

            Components.Add(new FramerateCounter(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.shader = this.Content.Load<Effect>("Shaders/Circle");
        }

        private void RegisterGameServices()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            this.elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //this.shader.Parameters["circleRadius"].SetValue(this.elapsed);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render to rendertarget
            this.graphics.GraphicsDevice.SetRenderTarget(this.renderTarget);          
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            this.graphics.GraphicsDevice.SetRenderTarget(null);

            // Render rendertarget to backbuffer
            spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                //this.shader);
                null);

            spriteBatch.Draw(this.renderTarget, this.renderTarget.Bounds, Color.White);
            
            spriteBatch.End();
        }
    }
}
