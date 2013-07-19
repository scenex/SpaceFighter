// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Input.Implementation;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

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

        SpriteBatch spriteBatch;
        private Effect shader;

        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;
        private RenderTarget2D renderTarget;

        private float elapsed;

        private readonly GraphicsDeviceManager graphics;

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
           
            this.renderTarget = new RenderTarget2D(this.GraphicsDevice, ScreenWidth, ScreenHeight);
            this.graphics.ApplyChanges();

            this.ComposeServices();

            #if WINDOWS           
            if(this.inputService.IsGamePadConnected)
            {
                this.inputService.SetInputDevice(new InputGamepad());
            }
            else
            {
                this.inputService.SetInputDevice(new InputKeyboard());
            }            
            #elif XBOX
                this.inputService.SetInputDevice(new InputGamepad());
            #endif

            Components.Add(new FramerateCounter(this));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.shader = this.Content.Load<Effect>("Shaders/Circle");
        }

        private void ComposeServices()
        {            
            //var translation = new Vector3(
            //    (this.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (this.playerService.Player.Width / 2) + 62,
            //    (this.GraphicsDevice.PresentationParameters.BackBufferHeight / 2)- (this.playerService.Player.Height / 2) + 32, 0);

            var translation = new Vector3(
                (this.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - (80 / 2) + 62,
                (this.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) - (80 / 2) + 32, 0); // Todo: Find solution for magic constants
            cameraService = new CameraService(this, translation);
            this.Components.Add(cameraService);

            terrainService = new TerrainService();
            this.Components.Add(terrainService);
            
            headUpDisplayService = new HeadUpDisplayService(this);
            this.Components.Add(headUpDisplayService);

            audioService = new AudioService(this);
            this.Components.Add(audioService);

            enemyFactory = new EnemyFactory(this, cameraService, terrainService);
            enemyService = new EnemyService(this, enemyFactory);
            this.Components.Add(enemyService);

            playerFactory = new PlayerFactory(this, this.cameraService);
            playerService = new PlayerService(this, audioService, playerFactory);
            this.Components.Add(playerService);

            inputService = new InputService(this, playerService);
            this.Components.Add(inputService);
            
            collisionDetectionService = new CollisionDetectionService(this, playerService, enemyService, terrainService);
            this.Components.Add(collisionDetectionService);

            debugService = new DebugService(this, cameraService);
            this.Components.Add(debugService);

            gameController = new GameController(this, collisionDetectionService, playerService, enemyService, inputService, headUpDisplayService, terrainService, debugService, audioService, cameraService);
            Components.Add(gameController);
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
