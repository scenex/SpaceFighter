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

        private readonly ITerrainService terrainService;
        private readonly IHeadUpDisplayService headUpDisplayService;
        private readonly IAudioService audioService;
        private readonly IPlayerService playerService;
        private readonly IInputService inputService;
        private readonly IEnemyService enemyService;
        private readonly ICollisionDetectionService collisionDetectionService;
        private readonly ICameraService cameraService;
        private readonly IDebugService debugService;
        private readonly IGameController gameController;

        private readonly GameStateManager gameStateManager = new GameStateManager();
        private readonly StateMachine<Action<double>> applicationStateMachine;
        private double elapsedTime;

        private MenuGameState menuGameState;
        private IntroGameState introGameState;
        private GameplayGameState gameplayGameState;

        private string selectedMenuItem;

        public ApplicationStateEngine(
            Game game, 
            ITerrainService terrainService,
            IHeadUpDisplayService headUpDisplayService,
            IAudioService audioService,
            IPlayerService playerService,
            IInputService inputService,
            IEnemyService enemyService,
            ICollisionDetectionService collisionDetectionService,
            ICameraService cameraService,
            IDebugService debugService,
            IGameController gameController)
        {
            this.game = game;
            this.terrainService = terrainService;
            this.headUpDisplayService = headUpDisplayService;
            this.audioService = audioService;
            this.playerService = playerService;
            this.inputService = inputService;
            this.enemyService = enemyService;
            this.collisionDetectionService = collisionDetectionService;
            this.cameraService = cameraService;
            this.debugService = debugService;
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
                        this.menuGameState = new MenuGameState(this.game, this.inputService);
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
                        this.gameplayGameState = new GameplayGameState(
                            this.game,
                            this.terrainService,
                            this.headUpDisplayService,
                            this.audioService,
                            this.playerService,
                            this.inputService,
                            this.enemyService,
                            this.collisionDetectionService,
                            this.cameraService,
                            this.debugService,
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
            this.gameStateManager.Push(new MenuGameState(this.game, this.inputService));
        }

        private void OnMenuItemSelected(object sender, MenuItemSelectedEventArgs menuItemSelectedEventArgs)
        {
            this.selectedMenuItem = menuItemSelectedEventArgs.SelectedMenuItem;
        }
    }
}
