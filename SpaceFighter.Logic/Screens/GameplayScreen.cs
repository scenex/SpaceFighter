// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayScreen : IScreenTransition
    {
        private readonly IGameController gameController;

        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }

        public GameplayScreen(IGameController gameController)
        {
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
            ((IUpdateable)this.gameController).Update(gameTime);

            if (this.gameController.IsGameRunning == false)
            {
                this.IsTransitionAllowed = true;
            }
        }

        public void Draw(GameTime gameTime)
        {
            ((IDrawable)this.gameController).Draw(gameTime);
        }
    }
}
