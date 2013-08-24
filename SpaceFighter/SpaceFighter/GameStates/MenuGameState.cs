// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    public class MenuGameState : GameState, IGameStateTransition, IDrawable
    {
        private readonly Game game;

        private readonly IInputService inputService;

        private MenuScreen menuScreen;

        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public object TransitionTag { get; private set; }

        public MenuGameState(Game game, IInputService inputService)
        {
            this.game = game;

            this.inputService = inputService;

            this.Visible = true;
        }

        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {
            this.inputService.InputStateHandling = InputStateHandling.Menu;
            this.game.Components.Add(this.inputService);

            this.menuScreen = new MenuScreen(this.game, this.inputService);
            this.menuScreen.Initialize();     
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.game.Components.Remove(this.inputService);
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            this.menuScreen.Update(gameTime);

            if (this.menuScreen.IsTransitionAllowed && (string)this.menuScreen.TransitionTag == MenuItems.StartGame)
            {
                this.IsTransitionAllowed = true;
                this.TransitionTag = MenuItems.StartGame;
            }

            if (this.menuScreen.IsTransitionAllowed && (string)this.menuScreen.TransitionTag == MenuItems.ExitGame)
            {
                this.IsTransitionAllowed = true;
                this.TransitionTag = MenuItems.ExitGame;
            }
        }

        public void Draw(GameTime gameTime)
        {
            this.menuScreen.Draw(gameTime);
        }
    }
}
