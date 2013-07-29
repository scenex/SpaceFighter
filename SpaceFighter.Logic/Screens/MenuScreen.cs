// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MenuScreen : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        public MenuScreen(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
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
            if (this.MenuItemSelected != null)
            {
                this.MenuItemSelected(this, new MenuItemSelectedEventArgs(MenuItems.StartGame));
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, "Start Game\nOptions\nExit Game", new Vector2(570, 500), Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
