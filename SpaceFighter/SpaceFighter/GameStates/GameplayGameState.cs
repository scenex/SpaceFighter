﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic.Screens;
    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayGameState : GameState, IGameStateTransition, IDrawable
    {
        private readonly GameplayScreen gameplayScreen;

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public bool Visible { get; private set; }
        public int DrawOrder { get; private set; }

        public GameplayGameState(IGameController gameController)
        {
            this.Visible = true;

            this.gameplayScreen = new GameplayScreen(gameController);
        }

        public object TransitionTag { get; private set; }
        public bool IsTransitionAllowed { get; private set; }

        protected override void OnEntered()
        {
            this.gameplayScreen.Initialize();
            this.gameplayScreen.StartGame();       
            base.OnEntered();
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
