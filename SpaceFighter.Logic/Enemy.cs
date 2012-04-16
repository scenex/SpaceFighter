﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The enemy class.
    /// </summary>
    public class Enemy : DrawableGameComponent, IEnemy
    {
        private Vector2 position;
        private readonly Game game;
        private readonly Texture2D enemySprite;
        private SpriteBatch spriteBatch;

        public Enemy(Game game) : base(game)
        {
            this.game = game;
            this.position = new Vector2(100, 100);
            this.enemySprite = this.game.Content.Load<Texture2D>("Sprites/Enemy");
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Texture2D EnemySprite
        {
            get
            {
                return this.enemySprite;
            }
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.enemySprite, this.position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }
    }
}
