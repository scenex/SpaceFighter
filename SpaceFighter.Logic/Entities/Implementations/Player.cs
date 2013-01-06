// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using System;
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
        private SpriteBatch spriteBatch;

        private Texture2D spriteAlive;
        private Texture2D spriteDead;

        private ICameraService cameraService;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.Position = startPosition;
            this.State = PlayerState.Alive;
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public PlayerState State { get; private set; }
        public Color[] ColorData { get; private set; }

        public int Width
        {
            get
            {
                return this.GetCurrentSprite().Width;
            }
        }

        public int Height
        {
            get
            {
                return this.GetCurrentSprite().Height;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(this.Position.X + (this.Width / 2.0f), this.Position.Y + (this.Height / 2.0f));
            }
        }

        public void Thrust(int amount)
        {
            this.Position =
                Vector2.Add(
                    new Vector2(
                        (float)Math.Cos(this.Rotation - MathHelper.PiOver2) * amount,
                        (float)Math.Sin(this.Rotation - MathHelper.PiOver2) * amount),
                    this.Position);
        }

        private Texture2D GetCurrentSprite()
        {
            switch (this.State)
            {
                case PlayerState.Alive:
                    return spriteAlive;

                case PlayerState.Dead:
                    return spriteDead;

                default:
                    throw new NotImplementedException();
            }
        }

        public void SetStateAlive()
        {
            this.State = PlayerState.Alive;
            this.UpdateSpriteColorData();

        }

        public void SetStateDead()
        {
            this.State = PlayerState.Dead;
            this.UpdateSpriteColorData();
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.spriteAlive = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive");
            this.spriteDead = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dead");

            this.UpdateSpriteColorData();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.cameraService.Position = this.Position;
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
                this.GetCurrentSprite(),
                this.Position,
                null,
                Color.White,
                this.Rotation,
                new Vector2((float)this.Width / 2, (float)this.Height / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateSpriteColorData()
        {
            // Obtain color information for subsequent per pixel collision detection
            this.ColorData = new Color[this.GetCurrentSprite().Width * this.GetCurrentSprite().Height];
            this.GetCurrentSprite().GetData(this.ColorData);
        }
    }

    public enum PlayerState
    {
        None,
        Alive,
        Dying,
        Dead
    }
}
