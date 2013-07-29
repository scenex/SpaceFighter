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
    using SpaceFighter.Logic.StateMachine;

    public class ApplicationStateEngine
    {
        private readonly Game game;

        private readonly GameStateManager gameStateManager = new GameStateManager();
        private readonly StateMachine<Action<double>> applicationStateMachine;
        private double elapsedTime;

        private MenuGameState menuGameState;
        private IntroGameState introGameState;
        private GameplayGameState gameplayGameState;

        private string selectedMenuItem;

        public ApplicationStateEngine(Game game)
        {
            this.game = game;

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
                        this.menuGameState = new MenuGameState(this.game);
                        this.menuGameState.MenuItemSelected += this.OnMenuItemSelected;
                        this.gameStateManager.Push(this.menuGameState);
                    }, 
                delegate
                    {
                        this.menuGameState.MenuItemSelected -= this.OnMenuItemSelected;
                        this.gameStateManager.Pop();
                    });

            var gameplay = new State<Action<double>>(
                "Gameplay",
                null,
                delegate
                    {
                        this.gameplayGameState = new GameplayGameState(this.game);
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
            
            this.applicationStateMachine = new StateMachine<Action<double>>(intro);

            EventAggregator.Subscribe(this, "GameOver");
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
            this.gameStateManager.Pop();
            this.gameStateManager.Push(new MenuGameState(this.game));
        }

        private void OnMenuItemSelected(object sender, MenuItemSelectedEventArgs menuItemSelectedEventArgs)
        {
            this.selectedMenuItem = menuItemSelectedEventArgs.SelectedMenuItem;
        }
    }
}
