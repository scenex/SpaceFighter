// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;
    using System.IO;
    using System.Reflection;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic;
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
        IGameController gameController;

        private readonly GraphicsDeviceManager graphics;

        private ApplicationController applicationController;

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

            this.applicationController = new ApplicationController(
                this,
                this.gameController,
                this.inputService);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //this.spriteBatch = new SpriteBatch(GraphicsDevice);
            //this.shader = this.Content.Load<Effect>("Shaders/Circle");
        }

        private void ComposeServices()
        {            
            this.terrainService = new TerrainService(
                this);

            this.headUpDisplayService = new HeadUpDisplayService(
                this);

            this.audioService = new AudioService(
                this);

            this.enemyFactory = new EnemyFactory(
                this,
                terrainService);

            this.enemyService = new EnemyService(
                this, 
                enemyFactory);

            this.playerFactory = new PlayerFactory(
                this);

            this.inputService = new InputService(
                this);

            this.playerService = new PlayerService(
                this, 
                inputService,
                audioService, 
                playerFactory,
                terrainService);

            this.collisionDetectionService = new CollisionDetectionService(
                this, 
                playerService, 
                enemyService, 
                terrainService);

            this.gameController = new GameController(
                this, 
                collisionDetectionService, 
                playerService, 
                enemyService, 
                inputService, 
                headUpDisplayService, 
                terrainService, 
                audioService);

            try
            {
                var consoleAssembly = Assembly.Load("SpaceFighter.Console");
                var consoleServiceType = consoleAssembly.GetType("SpaceFighter.Console.ConsoleService");            
                var consoleService = Activator.CreateInstance(consoleServiceType, this);

                this.Components.Add(consoleService as IGameComponent);
            }
            catch(FileNotFoundException)
            {
                // No console support available
            }
        }

        protected override void Update(GameTime gameTime)
        {
            this.applicationController.Update(gameTime);            
            base.Update(gameTime); // Re-Added
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);  // Re-Added

            this.applicationController.Draw(gameTime);
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
