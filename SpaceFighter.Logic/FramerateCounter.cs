// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class FramerateCounter : DrawableGameComponent
    {
        readonly ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate;
        int frameCounter;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FramerateCounter(Game game)
            : base(game)
        {
            this.content = new ContentManager(game.Services);
        }


        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.content.Load<SpriteFont>(@"Content\FramerateFont");
        }


        protected override void UnloadContent()
        {
            this.content.Unload();
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

            this.spriteBatch.DrawString(this.spriteFont, fps, new Vector2(25, 25), Color.White);

            this.spriteBatch.End();
        }
    }
}