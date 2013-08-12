// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter
{
    using System;

    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.GameStates;
    using SpaceFighter.Logic.EventManager;
    using SpaceFighter.Logic.Screens;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class ApplicationStateEngine
    {
        private readonly Game game;
        private readonly IInputService inputService;
        private readonly IGameController gameController;

        private readonly GameStateManager gameStateManager = new GameStateManager();
        private readonly StateMachine<Action<double>> applicationStateMachine;
        private double elapsedTime;

        private MenuGameState menuGameState;
        private IntroGameState introGameState;
        private GameplayGameState gameplayGameState;

        private string selectedMenuItem;

        private bool isGameOver;

        public ApplicationStateEngine(
            Game game, 
            IInputService inputService,
            IGameController gameController)
        {
            this.game = game;
            this.inputService = inputService;
            this.gameController = gameController;

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
                        this.isGameOver = false;

                        this.menuGameState = new MenuGameState(
                            this.game, 
                            this.inputService);

                        this.menuGameState.MenuItemSelected += this.OnMenuItemSelected;
                        this.gameStateManager.Push(this.menuGameState);
                    }, 
                delegate
                    {
                        this.menuGameState.MenuItemSelected -= this.OnMenuItemSelected;
                        this.selectedMenuItem = MenuItems.None;
                        this.gameStateManager.Pop();
                    });

            var gameplay = new State<Action<double>>(
                "Gameplay",
                null,
                delegate
                    {
                        this.gameplayGameState = new GameplayGameState(
                            this.game,
                            this.gameController);

                        this.gameStateManager.Push(this.gameplayGameState);
                    }, 
                () => this.gameStateManager.Pop());

            var exit = new State<Action<double>>(
                "Exit",
                null,
                () => this.game.Exit(),
                null);

            intro.AddTransition(menu, () => this.elapsedTime > 4000);
            menu.AddTransition(gameplay, () => this.selectedMenuItem == MenuItems.StartGame);
            menu.AddTransition(exit, () => this.selectedMenuItem == MenuItems.ExitGame);
            gameplay.AddTransition(menu, () => this.isGameOver);

            this.applicationStateMachine = new StateMachine<Action<double>>(intro);

            EventAggregator.Subscribe(this, "GameOver");
            EventAggregator.Subscribe(this, "LevelCompleted");
            EventAggregator.Subscribe(this, "PauseToggled");
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateManager.Update(gameTime);
            this.applicationStateMachine.Update();
        }

        public void Draw(GameTime gameTime)
        {
            this.gameStateManager.Draw(gameTime);
        }

        [Subscription("GameOver")]
        public void GameOverSubscriptionHandler()
        {
            this.isGameOver = true;
        }

        [Subscription("LevelCompleted")]
        public void LevelCompletedSubscriptionHandler()
        {
            this.gameController.EndGame();
            this.gameController.StartGame();
        }

        [Subscription("PauseToggled")]
        public void PauseToggledSubscriptionHandler()
        {
            if (this.inputService.IsGamePaused)
            {
                this.gameStateManager.Pause();
            }
            else
            {
                this.gameStateManager.Resume();
            }
        }

        private void OnMenuItemSelected(object sender, MenuItemSelectedEventArgs menuItemSelectedEventArgs)
        {
            this.selectedMenuItem = menuItemSelectedEventArgs.SelectedMenuItem;
        }
    }
}
