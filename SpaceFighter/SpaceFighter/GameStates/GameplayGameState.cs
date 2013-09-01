// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayGameState : GameState, IGameStateTransition, IDrawable
    {
        private readonly Game game;
        readonly IGameController gameController;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public GameplayGameState(Game game, IGameController gameController)
        {
            this.game = game;
            this.gameController = gameController;
            this.Visible = true;
        }

        public object TransitionTag { get; private set; }
        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {                     
            this.gameController.Initialize();
            this.gameController.StartGame();       
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.gameController.EndGame();
            this.game.Components.Clear();
            base.OnLeaving();
        }

        protected override void OnPause()
        {
            this.gameController.PauseGame();
            base.OnPause();
        }

        protected override void OnResume()
        {
            this.gameController.ResumeGame();
            base.OnResume();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            ((IUpdateable)this.gameController).Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            ((IDrawable)this.gameController).Draw(gameTime);
        }
    }
}
