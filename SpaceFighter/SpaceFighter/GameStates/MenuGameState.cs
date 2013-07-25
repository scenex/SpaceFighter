// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;

    public class MenuGameState : GameState
    {
        private readonly Game game;
        private MenuScreen menuScreen;

        public MenuGameState(Game game)
        {
            this.game = game;
        }

        protected override void OnEntered()
        {
            menuScreen = new MenuScreen(this.game);
            this.game.Components.Add(menuScreen);
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.game.Components.Remove(menuScreen);
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {

        }
    }
}
