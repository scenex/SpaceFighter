// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;

    public class IntroGameState : GameState, IGameStateTransition
    {
        private readonly Game game;
        private IntroScreen introScreen;
        public object TransitionTag { get; private set; }

        public IntroGameState(Game game)
        {
            this.game = game;
        }

        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {
            this.introScreen = new IntroScreen(this.game);
            this.game.Components.Add(this.introScreen);
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.game.Components.Remove(this.introScreen);
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            // Components do their own updating...

            if(this.introScreen.IsTransitionAllowed)
            {
                this.IsTransitionAllowed = true;
            }
        }
    }
}
