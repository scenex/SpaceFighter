// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using SpaceFighter.Logic.EventManager;
    using SpaceFighter.Logic.StateMachine;

    public class GameStateEngine
    {
        private readonly StateMachine<Action<int>> gameStateMachine;

        private int health;

        public GameStateEngine()
        {
            var gameStarted = new State<Action<int>>(
                "GameStarted",
                null,
                null,
                null);

            var gameOver = new State<Action<int>>(
                "GameOver",
                null,
                () => EventAggregator.Fire(this, "GameOver"),
                null);

            gameStarted.AddTransition(gameOver, () => this.health <= 0);
            this.gameStateMachine = new StateMachine<Action<int>>(gameStarted);
        }

        public void Update(int remainingHealth)
        {
            this.health = remainingHealth;
            this.gameStateMachine.Update();
        }
    }
}
