// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.EventManager;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class GameStateEngine
    {
        private readonly IPlayerService playerService;

        private readonly IEnemyService enemyService;

        private readonly IInputService inputService;

        private readonly StateMachine<Action<double>> gameStateMachine;
        
        private double elapsedTime;
        private double elapsedTimeSinceEndingTransition;

        public GameStateEngine(IPlayerService playerService, IEnemyService enemyService, IInputService inputService)
        {
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
                        EventAggregator.Fire(this, "LevelCompleted");
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
                () => EventAggregator.Fire(this, "GameOver"),
                null);

            starting.AddTransition(started, () => this.elapsedTime > 1500);
            started.AddTransition(ending, () => this.enemyService.IsBossEliminated);
            ending.AddTransition(ended, () => this.elapsedTime - this.elapsedTimeSinceEndingTransition > 1500);
            ended.AddTransition(starting, () => true);

            started.AddTransition(paused, () => this.inputService.IsGamePaused == true);
            paused.AddTransition(started, () => this.inputService.IsGamePaused == false);
            
            started.AddTransition(gameOver, () => this.playerService.Player.Health <= 0);
            
            this.gameStateMachine = new StateMachine<Action<double>>(starting);
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            this.gameStateMachine.Update();
        }
    }
}