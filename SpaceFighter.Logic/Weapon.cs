// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Weapon : DrawableGameComponent, IWeapon
    {
        private readonly List<Vector2> spritePositions;
        private SpriteBatch spriteBatch;

        public Weapon(Game game) : base(game)
        {
            this.spritePositions = new List<Vector2>();
        }

        public void FireWeapon(Vector2 startPosition)
        {
            this.spritePositions.Add(new Vector2(
                startPosition.X - ((float)this.Sprite.Width / 2),
                startPosition.Y - ((float)this.Sprite.Width / 2)));
        }

        public IEnumerable<Vector2> SpritePositions
        {
            get
            {
                return this.spritePositions;
            }
        }

        public Texture2D Sprite { get; private set; }

        public Color[] SpriteDataCached { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Sprite = this.Game.Content.Load<Texture2D>("Sprites/Spaceship_Shot");

            //// Obtain color information for subsequent per pixel collision detection
            this.SpriteDataCached = new Color[this.Sprite.Width * this.Sprite.Height];
            this.Sprite.GetData(this.SpriteDataCached);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            for (var i = 0; i < this.spritePositions.Count; i++)
            {
                if (this.spritePositions[i].Y >= 0)
                {
                    this.spritePositions[i] = new Vector2(this.spritePositions[i].X, this.spritePositions[i].Y - 5);
                }
                else
                {
                    // Remove shots which are not visible anymore.
                    this.spritePositions.Remove(this.spritePositions[i]);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            foreach (var shotPosition in this.spritePositions)
            {
                this.spriteBatch.Draw(this.Sprite, shotPosition, Color.White);
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
