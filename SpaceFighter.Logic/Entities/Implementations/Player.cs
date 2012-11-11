// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;

    /// <summary>
    /// The spaceship class which represent the player's spaceship.
    /// </summary>
    public class Player : DrawableGameComponent, IPlayer
    {   
        private readonly Game game;
        private Texture2D sprite;
        private SpriteBatch spriteBatch;
        private Color[] spriteDataCached;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.Position = startPosition;
        }

        public Vector2 Position { get; set; }

        public int Width
        {
            get
            {
                return this.sprite.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.sprite.Height;
            }
        }

        public Color[] ColorData
        {
            get
            {
                return this.spriteDataCached;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(this.Position.X + (this.Width / 2.0f), this.Position.Y + (this.Height / 2.0f));
            }
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.sprite = this.game.Content.Load<Texture2D>("Sprites/Spaceship");

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.sprite.Width * this.sprite.Height];
            this.sprite.GetData(this.spriteDataCached);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.sprite, this.Position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
