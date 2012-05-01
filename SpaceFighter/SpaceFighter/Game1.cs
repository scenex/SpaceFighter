// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Services;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Enemy enemy;

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private IPlayerService playerService; 

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
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
            this.graphics.ApplyChanges();

            this.enemy = new Enemy(this);

            this.Components.Add(this.enemy);

            this.Services.AddService(typeof(IPlayerService), new PlayerService(this));
            this.playerService = (IPlayerService)this.Services.GetService(typeof(IPlayerService));

            this.Services.AddService(typeof(IEnemiesServices), new EnemiesService(this));

            this.Services.AddService(typeof(ICollisionDetectionService), new CollisionDetectionService(this));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
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
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (this.playerService.Player.Position.X >= 0)
                {
                    this.playerService.MoveLeft();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (this.playerService.Player.Position.X + this.playerService.Player.ShipSprite.Width <= ScreenWidth)
                {
                    this.playerService.MoveRight();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (this.playerService.Player.Position.Y - 3 >= 0)
                {
                    this.playerService.MoveUp();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (this.playerService.Player.Position.Y + this.playerService.Player.ShipSprite.Height <= ScreenHeight)
                {
                    this.playerService.MoveDown();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.LeftControl) && this.previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                this.playerService.FireWeapon();
            }

            this.previousKeyboardState = this.currentKeyboardState;

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
