// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Primitives;

    public class IntroScreen : DrawableGameComponent, IScreenTransition
    {
        private double elapsedTime;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Curve introTextFade;
        private CubePrimitive cube;
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
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");
            this.introTextFade = this.Game.Content.Load<Curve>(@"Curves\IntroTextFade");
            this.cube = new CubePrimitive(this.GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.elapsedTime > 5000)
            {
                this.IsTransitionAllowed = true;
                this.TransitionTag = null;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.SetRenderTarget(this.renderTarget);
            this.GraphicsDevice.Clear(Color.Black);

            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(
                this.spriteFont,
                "Cataclysm Game Studios Presents",
                new Vector2(470, 200),
                Color.White * this.introTextFade.Evaluate((float)elapsedTime / 1000));

            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            var yaw = time * 0.4f;
            var pitch = time * 0.7f;
            var roll = time * 1.1f;

            var cameraPosition = new Vector3(0, 0, 5.0f);

            float aspect = GraphicsDevice.Viewport.AspectRatio;

            var world = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            var view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);

            this.cube.Draw(world, view, projection, Color.White);

            this.spriteBatch.End();

            this.GraphicsDevice.SetRenderTarget(null);

            // Render rendertarget to backbuffer
            spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null);
            spriteBatch.Draw(this.renderTarget, this.renderTarget.Bounds, Color.White * this.introTextFade.Evaluate((float)elapsedTime / 1000));
            spriteBatch.End();
        }
    }
}