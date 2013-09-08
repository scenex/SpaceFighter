// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using System.Diagnostics;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.EventManager;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class GameStateEngine
    {
        private readonly IGameController gameController;

        private readonly IPlayerService playerService;

        private readonly IEnemyService enemyService;

        private readonly IInputService inputService;

        private readonly StateMachine<Action<double>> gameStateMachine;
        
        private double elapsedTime;
        private double elapsedTimeSinceEndingTransition;

        private bool reset;

        public GameStateEngine(IGameController gameController, IPlayerService playerService, IEnemyService enemyService, IInputService inputService)
        {
            this.gameController = gameController;
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.inputService = inputService;

            var starting = new State<Action<double>>(
                "Starting",
                null,
                null,
                null);

            var started = new State<Action<double>>(
                "Started",
                null,
                null,
                null);

            var ending = new State<Action<double>>(
                "Ending",
                null,
                () => this.elapsedTimeSinceEndingTransition = elapsedTime,
                null);

            var ended = new State<Action<double>>(
                "Ended",
                null,
                delegate
                    {
                        this.elapsedTime = 0;
                        this.elapsedTimeSinceEndingTransition = 0;

                        this.gameController.EndGame();
                        this.gameController.StartGame();
                    }, 
                null);

            var paused = new State<Action<double>>(
                "Paused",
                null,
                () => EventAggregator.Fire(this, "PauseToggled"),
                () => EventAggregator.Fire(this, "PauseToggled"));

            var gameOver = new State<Action<double>>(
                "GameOver",
                null,
                null,
                () => this.reset = false);

            starting.AddTransition(started, () => this.elapsedTime > 1500);
            started.AddTransition(ending, () => this.enemyService.IsBossEliminated);
            ending.AddTransition(ended, () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > 1500);
            ended.AddTransition(starting, () => true);

            started.AddTransition(paused, () => this.inputService.IsGamePaused == true);
            paused.AddTransition(started, () => this.inputService.IsGamePaused == false);
            
            started.AddTransition(gameOver, () => this.playerService.Player.Health <= 0);

            gameOver.AddTransition(starting, () => this.reset);
            
            this.gameStateMachine = new StateMachine<Action<double>>(starting);
        }

        public string CurrentState 
        { 
            get
            {
                return this.gameStateMachine.CurrentState.Name;
            }
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateMachine.Update();

            ((IUpdateable)this.gameController).Update(gameTime);

            Debug.WriteLine(this.gameStateMachine.CurrentState.Name);
        }

        public void Draw(GameTime gameTime)
        {
            ((IDrawable)this.gameController).Draw(gameTime);
        }

        public void Reset()
        {
            this.elapsedTime = 0;
            this.reset = true;
        }

        public void Initialize()
        {
            this.gameController.Initialize();
        }

        public void StartGame()
        {
            this.gameController.StartGame();
        }

        public void EndGame()
        {
            this.gameController.EndGame();
        }

        public void PauseGame()
        {
            this.gameController.PauseGame();
        }

        public void ResumeGame()
        {
            this.gameController.ResumeGame();
        }
    }
}