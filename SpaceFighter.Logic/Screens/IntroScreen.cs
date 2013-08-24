﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class IntroScreen : DrawableGameComponent, IScreenTransition
    {
        private double elapsedTime;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private RenderTarget2D renderTarget;

        public IntroScreen(Game game) : base(game)
        {
        }

        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }

        public override void Initialize()
        {
            base.Initialize();
            this.renderTarget = new RenderTarget2D(this.GraphicsDevice, this.GraphicsDevice.PresentationParameters.BackBufferWidth, this.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"FramerateFont");
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsedTime > 4000)
            {
                this.IsTransitionAllowed = true;
                this.TransitionTag = null;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //this.GraphicsDevice.SetRenderTarget(this.renderTarget);
            //this.GraphicsDevice.Clear(Color.Black);

            //this.spriteBatch.Begin();

            spriteBatch.Begin();

            this.spriteBatch.DrawString(
                this.spriteFont, 
                "Cataclysm Game Studios Presents\n\n         Space Fighter", 
                new Vector2(400, 300), 
                Color.White * 0.2f);

            this.spriteBatch.End();

            //this.GraphicsDevice.SetRenderTarget(null);

            //// Render rendertarget to backbuffer
            //spriteBatch.Begin(
            //    SpriteSortMode.BackToFront,
            //    BlendState.AlphaBlend,
            //    null,
            //    null,
            //    null,
            //    null);
            //spriteBatch.Draw(this.renderTarget, this.renderTarget.Bounds, fadeColor);
            //spriteBatch.End();
        }
    }
}