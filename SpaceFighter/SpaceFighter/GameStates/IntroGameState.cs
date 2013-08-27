// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;

    public class IntroGameState : GameState, IGameStateTransition, IDrawable
    {
        private readonly Game game;

        private IntroScreen introScreen;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public object TransitionTag { get; private set; }

        public IntroGameState(Game game)
        {
            this.game = game;
            this.Visible = true;
        }

        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {
            this.introScreen = new IntroScreen(this.game);
            this.introScreen.Initialize();
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.introScreen.Dispose();
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            this.introScreen.Update(gameTime);

            if(this.introScreen.IsTransitionAllowed)
            {
                this.IsTransitionAllowed = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            this.introScreen.Draw(gameTime);
        }
    }
}
