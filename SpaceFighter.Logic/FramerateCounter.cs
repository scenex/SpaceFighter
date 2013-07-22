// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FramerateCounter : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate;
        int frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FramerateCounter(Game game) : base(game)
        {
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"FramerateFont");
        }

        protected override void UnloadContent()
        {

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

        public override void Draw(GameTime gameTime)
        {
            this.frameCounter++;

            string fps = string.Format("FPS:{0}", this.frameRate);

            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(this.spriteFont, fps, new Vector2(50, 50), Color.White);

            this.spriteBatch.End();
        }
    }
}