// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class DebugService : DrawableGameComponent, IDebugService
    {
        private SpriteBatch spriteBatch;
        private ICameraService cameraService;

        private Rectangle rectangle;
        private Texture2D texture;

        public DebugService(Game game, ICameraService cameraService) : base(game)
        {
            this.cameraService = cameraService;
            this.rectangle = new Rectangle();
        }

        protected override void  LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.texture = this.Game.Content.Load<Texture2D>("Sprites/Debugging/Rectangle");
 	        base.LoadContent();
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

                this.spriteBatch.Draw(this.texture, this.rectangle, Color.White);

                this.spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void DrawRectangle(Rectangle rect)
        {
            this.rectangle = rect;
        }
    }
}
