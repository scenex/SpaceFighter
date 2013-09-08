// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using Microsoft.Xna.Framework;

    public class GameplayScreen : IScreenTransition
    {
        private readonly GameStateEngine gameStateEngine;

        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }

        public GameplayScreen(GameStateEngine gameStateEngine)
        {
            this.gameStateEngine = gameStateEngine;
        }

        public void Initialize()
        {
            this.gameStateEngine.Initialize();
        }

        public void StartGame()
        {
            this.gameStateEngine.StartGame();
        }

        public void EndGame()
        {
            this.gameStateEngine.EndGame();
        }

        public void PauseGame()
        {
            this.gameStateEngine.PauseGame();
        }

        public void ResumeGame()
        {
            this.gameStateEngine.ResumeGame();
        }

        public void Update(GameTime gameTime)
        {
            this.gameStateEngine.Update(gameTime);

            if(gameStateEngine.CurrentState == "GameOver") // Todo: Change to use name dictionary
            {
                this.IsTransitionAllowed = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            this.gameStateEngine.Draw(gameTime);
        }
    }
}
