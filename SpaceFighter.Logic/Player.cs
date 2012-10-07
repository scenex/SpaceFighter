// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The spaceship class which represent the players spaceship.
    /// </summary>
    public class Player : DrawableGameComponent, IPlayer
    {   
        private readonly Game game;
        private readonly Texture2D sprite;
        private SpriteBatch spriteBatch;
        private Color[] spriteDataCached;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.Position = startPosition;
            this.sprite = this.game.Content.Load<Texture2D>("Sprites/Spaceship");
        }

        public Vector2 Position { get; set; }

        public Texture2D Sprite
        {
            get
            {
                return this.sprite;
            }
        }

        public Texture2D ExplosionSequence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Color[] SpriteDataCached
        {
            get
            {
                return this.spriteDataCached;
            }
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.sprite.Width * this.sprite.Height];
            this.sprite.GetData(spriteDataCached);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.Sprite, this.Position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
