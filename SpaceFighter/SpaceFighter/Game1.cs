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

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Spaceship spaceship;

        private const int screenWidth = 640;

        private const int screenHeight = 480;

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
            this.graphics.PreferredBackBufferWidth = screenWidth;
            this.graphics.PreferredBackBufferHeight = screenHeight;
            this.graphics.ApplyChanges();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.spaceship = new Spaceship(this);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (this.spaceship.Position.X + this.spaceship.SpriteRegular.Width <= screenWidth)
                {
                    this.spaceship.MoveRight();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (this.spaceship.Position.X >= 0)
                {
                    this.spaceship.MoveLeft();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (this.spaceship.Position.Y >= 0)
                {
                    this.spaceship.MoveUp();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (this.spaceship.Position.Y + this.spaceship.SpriteRegular.Height <= screenHeight)
                {
                    this.spaceship.MoveDown();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.spaceship.SpriteRegular, this.spaceship.Position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
