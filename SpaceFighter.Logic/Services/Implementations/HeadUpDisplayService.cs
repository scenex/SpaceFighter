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

        private Texture2D border;

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

            this.border = new Texture2D(this.GraphicsDevice, 160, 720);
            var data = new Color[160 * 720];
            for (var i = 0; i < data.Length; ++i) data[i] = Color.DarkBlue;
            this.border.SetData(data);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            this.spriteBatch.Draw(this.border, Vector2.Zero, Color.White);
            this.spriteBatch.Draw(this.border, new Vector2(1280 - 160, 0), Color.White);

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
