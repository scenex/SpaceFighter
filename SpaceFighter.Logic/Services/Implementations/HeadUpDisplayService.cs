// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Services.Interfaces;

    public class HeadUpDisplayService : DrawableGameComponent, IHeadUpDisplayService
    {
        private readonly PrimitiveBatch primitiveBatch;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public HeadUpDisplayService(Game game) : base(game)
        {
            this.Health = 100;
            this.primitiveBatch = new PrimitiveBatch(game.GraphicsDevice);
        }

        public int Health { get; set; }
        public int Lives { get; set; }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(this.spriteFont, "Energy", new Vector2(15, 20), Color.White);
            this.spriteBatch.DrawString(this.spriteFont, "Lives: " + this.Lives, new Vector2(15, 100), Color.White);
            this.spriteBatch.End();

            this.DrawEnergyBar(15, 50);
            base.Draw(gameTime);
        }

        public void DrawEnergyBar(int x, int y)
        {
            this.primitiveBatch.Begin(PrimitiveType.LineList);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 0  + y), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 0  + y), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 0  + y), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(0   + x, 20 + y), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(110 + x, 0  + y), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 20 + y), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 20 + y), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 20 + y), Color.White);

            for (int i = 0; i < this.Health; i++)
            {
                this.primitiveBatch.AddVertex(new Vector2(5 + i + x, 5  + y), Color.White);
                this.primitiveBatch.AddVertex(new Vector2(5 + i + x, 15 + y), Color.White);
            }

            this.primitiveBatch.End();
        }        
    }
}
