// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Input.Implementation;
    using SpaceFighter.Logic.Services.Implementations;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game
    {
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;

        private readonly GraphicsDeviceManager graphics;

        private GameController gameController;

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
            this.graphics.ApplyChanges();

            this.gameController = new GameController(this);
            Components.Add(this.gameController);
            Components.Add(new FramerateCounter(this));

            #if WINDOWS
                this.gameController.SetInputDevice(new InputKeyboard());
            #elif XBOX
                this.gameController.SetInputDevice(new InputGamepad());
            #endif

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
