// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Console
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SpaceFighter.Interfaces;

    public class ConsoleService : DrawableGameComponent, IConsoleService
    {
        private SpriteBatch spriteBatch;

        private SpriteFont spriteFont;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private readonly ISpaceFighterApi spaceFighterApi;

        private bool isActive;

        public ConsoleService(Game game) : base(game)
        {
            this.isActive = true;
            this.spaceFighterApi = new SpaceFighterApi();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Tab) && this.previousKeyboardState.IsKeyUp(Keys.Tab))
            {
                this.isActive = !this.isActive;
            }

            this.previousKeyboardState = this.currentKeyboardState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.isActive)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.DrawString(this.spriteFont, "Supervisor console active..", new Vector2(50, 50), Color.White);
                this.spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}
