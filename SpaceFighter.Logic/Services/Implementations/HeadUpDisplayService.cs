// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Services.Interfaces;

    public class HeadUpDisplayService : DrawableGameComponent, IHeadUpDisplayService
    {
        private readonly PrimitiveBatch primitiveBatch;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        int frameRate;
        int frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;

        private Texture2D screenBorder;

        public HeadUpDisplayService(Game game) : base(game)
        {
            this.Visible = false; // no automatic drawing

            this.Health = 100;
            this.primitiveBatch = new PrimitiveBatch(game.GraphicsDevice);
        }

        public int Health { get; set; }
        public int Lives { get; set; }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");

            this.screenBorder = new Texture2D(this.GraphicsDevice, 160, 720);
            var data = new Color[160 * 720];
            for (var i = 0; i < data.Length; ++i) data[i] = Color.DarkBlue;
            this.screenBorder.SetData(data);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime;

            if (this.elapsedTime > TimeSpan.FromSeconds(1))
            {
                this.elapsedTime -= TimeSpan.FromSeconds(1);
                this.frameRate = this.frameCounter;
                this.frameCounter = 0;
            }   
        }

        /// <summary>
        /// Special Draw call, intended to be called manually (shall not be part of game render target).
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="color"></param>
        public void Draw(GameTime gameTime, Color color)
        {
            this.DrawScreenBorders(color);
            this.DrawVitals(color);
            this.DrawEnergyBar(15, 50, color);
            this.DrawFps(color);

            base.Draw(gameTime);
        }

        private void DrawScreenBorders(Color color)
        {
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.screenBorder, Vector2.Zero, color);
            this.spriteBatch.Draw(this.screenBorder, new Vector2(1280 - 160, 0), color);
            this.spriteBatch.End();
        }

        private void DrawVitals(Color color)
        {
            this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.DrawString(this.spriteFont, "Energy", new Vector2(15, 20), color);
            this.spriteBatch.DrawString(this.spriteFont, "Lives: " + this.Lives, new Vector2(15, 100), color);
            this.spriteBatch.End();
        }

        public void DrawEnergyBar(int x, int y, Color color)
        {
            this.primitiveBatch.Begin(PrimitiveType.LineList);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 0  + y), color);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 0  + y), color);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 0  + y), color);
            this.primitiveBatch.AddVertex(new Vector2(0   + x, 20 + y), color);

            this.primitiveBatch.AddVertex(new Vector2(110 + x, 0  + y), color);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 20 + y), color);

            this.primitiveBatch.AddVertex(new Vector2(0   + x, 20 + y), color);
            this.primitiveBatch.AddVertex(new Vector2(110 + x, 20 + y), color);

            for (int i = 0; i < this.Health; i++)
            {
                this.primitiveBatch.AddVertex(new Vector2(5 + i + x, 5  + y), color);
                this.primitiveBatch.AddVertex(new Vector2(5 + i + x, 15 + y), color);
            }

            this.primitiveBatch.End();
        }

        private void DrawFps(Color color)
        {
            this.frameCounter++;
            var fps = string.Format("FPS:{0}", this.frameRate);
            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, fps, new Vector2(1170, 30), color);
            this.spriteBatch.End();
        }
    }
}
