// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayGameState : GameState
    {
        private readonly Game game;

        readonly IGameController gameController;

        public GameplayGameState(Game game, IGameController gameController)
        {
            this.game = game;
            this.gameController = gameController;
        }

        protected override void OnEntered()
        {                   
            this.game.Components.Add(this.gameController);           
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
            // Components do their own updating...
        }
    }
}
