// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// The spaceship class which represent the player's spaceship.
    /// </summary>
    public class Player : DrawableGameComponent, IPlayer
    {   
        private readonly Game game;
        private Texture2D sprite;
        private SpriteBatch spriteBatch;
        private Color[] spriteDataCached;
        private Vector2 position;
        private ICameraService cameraService;
        private float rotation;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.Position = startPosition;
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

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

        public float Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                this.rotation = value;
            }
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
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

        public override void Update(GameTime gameTime)
        {
            this.cameraService.Position = this.position;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            this.spriteBatch.Draw(
                this.sprite,
                this.Position,
                null,
                Color.White,
                this.rotation,
                new Vector2((float)this.Width / 2, (float)this.Height / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
