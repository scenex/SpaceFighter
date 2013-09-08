// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Screens;

    public class GameplayGameState : GameState, IGameStateTransition, IDrawable
    {
        private readonly Game game;
        private readonly GameplayScreen gameplayScreen;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public GameplayGameState(Game game, GameStateEngine gameStateEngine)
        {
            this.game = game;
            this.Visible = true;

            this.gameplayScreen = new GameplayScreen(gameStateEngine);
        }

        public object TransitionTag { get; private set; }
        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {
            this.gameplayScreen.Initialize();
            this.gameplayScreen.StartGame();       
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.gameplayScreen.EndGame();
            this.game.Components.Clear();
            base.OnLeaving();
        }

        protected override void OnPause()
        {
            this.gameplayScreen.PauseGame();
            base.OnPause();
        }

        protected override void OnResume()
        {
            this.gameplayScreen.ResumeGame();
            base.OnResume();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            this.gameplayScreen.Update(gameTime);

            if(this.gameplayScreen.IsTransitionAllowed)
            {
                this.IsTransitionAllowed = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            this.gameplayScreen.Draw(gameTime);
        }
    }
}
