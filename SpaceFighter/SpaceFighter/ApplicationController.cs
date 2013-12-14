// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.GameStates;
    using SpaceFighter.Logic.Screens;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class ApplicationController
    {
        private readonly Game game;

        private readonly IGameController gameController;

        private readonly IInputService inputService;

        private readonly GameStateManager gameStateManager = new GameStateManager();
        private readonly StateMachine<Action<double>> applicationStateMachine;

        private MenuGameState menuGameState;
        private IntroGameState introGameState;
        private GameplayGameState gameplayGameState;

        public ApplicationController(
            Game game,
            IGameController gameController,
            IInputService inputService)
        {
            this.game = game;
            this.gameController = gameController;
            this.inputService = inputService;

            var intro = new State<Action<double>>(
                "Intro",
                null,
                delegate
                    {
                        this.introGameState = new IntroGameState(this.game);
                        this.gameStateManager.Push(this.introGameState);
                    }, 
                () => this.gameStateManager.Pop());

            var menu = new State<Action<double>>(
                "Menu",
                null,
                delegate
                    {
                        this.menuGameState = new MenuGameState(this.game, this.inputService);
                        this.gameStateManager.Push(this.menuGameState);
                    },
                () => this.gameStateManager.Pop());

            var gameplay = new State<Action<double>>(
                "Gameplay",
                null,
                delegate
                    {
                        this.gameplayGameState = new GameplayGameState(this.gameController);
                        this.gameStateManager.Push(this.gameplayGameState);
                    }, 
                () => this.gameStateManager.Pop());

            var exit = new State<Action<double>>(
                "Exit",
                null,
                () => this.game.Exit(),
                null);

            intro.AddTransition(menu, () => this.introGameState.IsTransitionAllowed);
            menu.AddTransition(gameplay, () => this.menuGameState.IsTransitionAllowed && (string)this.menuGameState.TransitionTag == MenuItems.StartGame);
            menu.AddTransition(exit, () => this.menuGameState.IsTransitionAllowed && (string)this.menuGameState.TransitionTag == MenuItems.ExitGame);
            gameplay.AddTransition(menu, () => this.gameplayGameState.IsTransitionAllowed);

            //this.applicationStateMachine = new StateMachine<Action<double>>(intro);
            this.applicationStateMachine = new StateMachine<Action<double>>(gameplay); // Skipping intro and menu for faster startup
        }

        public void Update(GameTime gameTime)
        {
            this.gameStateManager.Update(gameTime);
            this.applicationStateMachine.Update();
        }

        public void Draw(GameTime gameTime)
        {
            this.gameStateManager.Draw(gameTime);
        }
    }
}
