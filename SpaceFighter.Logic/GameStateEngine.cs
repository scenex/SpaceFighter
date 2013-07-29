// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

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
                OnEnter,
                null);

            gameStarted.AddTransition(gameOver, () => this.health == 0);
            this.gameStateMachine = new StateMachine<Action<int>>(gameStarted);
        }

        private void OnEnter()
        {
            // Todo
            // Notify application state engine about Game Over
        }

        public void Update(int remainingHealth)
        {
            this.health = remainingHealth;
            this.gameStateMachine.Update();
        }
    }
}
