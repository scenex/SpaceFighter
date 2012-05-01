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
        private readonly Texture2D shipSprite;
        private SpriteBatch spriteBatch;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.Position = startPosition;
            this.shipSprite = this.game.Content.Load<Texture2D>("Sprites/Spaceship");
        }

        public event EventHandler CollisionDetected;

        public Vector2 Position { get; set; }

        public Texture2D ShipSprite
        {
            get
            {
                return this.shipSprite;
            }
        }

        public Texture2D ShipExplosionSequence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.ShipSprite, this.Position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
