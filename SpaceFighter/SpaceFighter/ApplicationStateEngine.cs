// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.GameStates;
    using SpaceFighter.Logic.StateMachine;

    public class ApplicationStateEngine
    {
        private readonly Game game;

        private readonly GameStateManager gameStateManager = new GameStateManager();
        private readonly StateMachine<Action<double>> applicationStateMachine;
        private double elapsedTime;

        public ApplicationStateEngine(Game game)
        {
            this.game = game;

            var intro = new State<Action<double>>(
                "Intro",
                null,
                null,
                () => this.gameStateManager.Pop());

            var menu = new State<Action<double>>(
                "Menu",
                null,
                () => this.gameStateManager.Push(new MenuGameState(this.game)),
                () => this.gameStateManager.Pop());

            var gameplay = new State<Action<double>>(
                "Gameplay",
                null,
                () => this.gameStateManager.Push(new GameplayGameState(this.game)),
                () => this.gameStateManager.Pop());

            intro.AddTransition(menu, () => this.elapsedTime > 4000);
            menu.AddTransition(gameplay, () => this.elapsedTime > 8000);

            this.applicationStateMachine = new StateMachine<Action<double>>(intro);
        }

        public void Update(GameTime gameTime)
        {
            if (this.gameStateManager.ActiveState == null)
            {
                this.gameStateManager.Push(new IntroGameState(this.game));
            }

            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateManager.Update(gameTime);
            this.applicationStateMachine.Update();
        }

        public void Draw(GameTime gameTime)
        {
            this.gameStateManager.Draw(gameTime);
        }
    }
}
