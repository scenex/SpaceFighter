// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class MenuScreen : DrawableGameComponent
    {
        private readonly IInputService inputService;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        private int selectionIndex;

        public MenuScreen(Game game, IInputService inputService) : base(game)
        {
            this.inputService = inputService;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"FramerateFont"); 

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.MenuItemSelected != null)
            {
                if (this.inputService.IsSelectionConfirmed)
                {
                    this.inputService.IsSelectionConfirmed = false;

                    if (this.GetSelectionIndex() == 0)
                    {
                        this.MenuItemSelected(this, new MenuItemSelectedEventArgs(MenuItems.StartGame));
                    }

                    if (this.GetSelectionIndex() == 1)
                    {
                        this.MenuItemSelected(this, new MenuItemSelectedEventArgs(MenuItems.ExitGame));
                    }
                }

                if (this.inputService.IsSelectionMoveDown)
                {
                    this.selectionIndex++;
                    this.inputService.IsSelectionMoveDown = false;
                }

                if (this.inputService.IsSelectionMoveUp)
                {
                    this.selectionIndex--;
                    this.inputService.IsSelectionMoveUp = false;
                }
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, "Start Game", new Vector2(570, 500), this.GetSelectionIndex() == 0 ? Color.Red : Color.White);
            this.spriteBatch.DrawString(this.spriteFont, "Exit Game", new Vector2(570, 550), this.GetSelectionIndex() == 1 ? Color.Red : Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private int GetSelectionIndex()
        {
            return Math.Abs(this.selectionIndex % 2);
        }
    }
}
