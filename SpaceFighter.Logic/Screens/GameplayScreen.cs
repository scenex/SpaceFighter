// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayScreen : IScreenTransition
    {
        private readonly GameStateEngine gameStateEngine;
        private readonly IGameController gameController;

        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }

        public GameplayScreen(GameStateEngine gameStateEngine, IGameController gameController)
        {
            this.gameStateEngine = gameStateEngine;
            this.gameController = gameController;
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

        public void Update(GameTime gameTime)
        {
            this.gameStateEngine.Update(gameTime);
            ((IUpdateable)this.gameController).Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            ((IDrawable)this.gameController).Draw(gameTime);
        }
    }
}
