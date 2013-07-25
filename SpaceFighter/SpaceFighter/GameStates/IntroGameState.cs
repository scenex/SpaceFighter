// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;

    public class IntroGameState : GameState
    {
        private readonly Game game;
        private IntroScreen introScreen;

        public IntroGameState(Game game)
        {
            this.game = game;
        }

        protected override void OnEntered()
        {
            introScreen = new IntroScreen(this.game);
            this.game.Components.Add(introScreen);
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.game.Components.Remove(introScreen);
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
