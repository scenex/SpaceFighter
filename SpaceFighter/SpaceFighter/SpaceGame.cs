// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Input.Implementation;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceGame : Game
    {
        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 720;

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
            this.graphics.ApplyChanges();

            this.RegisterGameServices();

            Components.Add(new FramerateCounter(this));

            #if WINDOWS
                ((IInputService)(this.Services.GetService(typeof(IInputService)))).SetInputDevice(new InputKeyboard());
            #elif XBOX
                ((IInputService)(this.Services.GetService(typeof(IInputService)))).SetInputDevice(new InputGamepad());
            #endif

            base.Initialize();
        }

        private void RegisterGameServices()
        {
            var gameController = new GameController(this);
            this.Services.AddService(typeof(IGameController), gameController);
            Components.Add(gameController);

            var inputService = new InputService(this);
            this.Services.AddService(typeof(IInputService), inputService);
            this.Components.Add(inputService);

            var collisionDetectionService = new CollisionDetectionService(this);
            this.Services.AddService(typeof(ICollisionDetectionService), collisionDetectionService);
            this.Components.Add(collisionDetectionService);

            var playerService = new PlayerService(this);
            this.Services.AddService(typeof(IPlayerService), playerService);
            this.Components.Add(playerService);

            var enemyService = new EnemyService(this);
            this.Services.AddService(typeof(IEnemyService), enemyService);
            this.Components.Add(enemyService);

            var playerWeaponService = new PlayerWeaponService(this);
            this.Services.AddService(typeof(IPlayerWeaponService), playerWeaponService);
            this.Components.Add(playerWeaponService);

            var enemyWeaponService = new EnemyWeaponService(this);
            this.Services.AddService(typeof(IEnemyWeaponService), enemyWeaponService);
            this.Components.Add(enemyWeaponService);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            base.Draw(gameTime);
        }
    }
}
