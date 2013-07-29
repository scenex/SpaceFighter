// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;

    public class MenuGameState : GameState, IDrawable
    {
        private readonly Game game;
        private MenuScreen menuScreen;

        public event EventHandler<MenuItemSelectedEventArgs> MenuItemSelected;

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public MenuGameState(Game game)
        {
            this.game = game;
            this.Visible = true;
        }

        protected override void OnEntered()
        {
            this.menuScreen = new MenuScreen(this.game);
            this.menuScreen.Initialize();

            this.menuScreen.MenuItemSelected += this.OnMenuItemSelected;
            
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.menuScreen.MenuItemSelected -= this.OnMenuItemSelected;
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            this.menuScreen.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            this.menuScreen.Draw(gameTime);
        }

        private void OnMenuItemSelected(object sender, MenuItemSelectedEventArgs menuItemSelectedEventArgs)
        {
            if (this.MenuItemSelected != null)
            {
                this.MenuItemSelected(sender, menuItemSelectedEventArgs);
            }
        }
    }
}
