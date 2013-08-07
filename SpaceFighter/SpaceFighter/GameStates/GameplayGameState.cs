// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.GameStates
{
    using Microsoft.Xna.Framework;

    using Nuclex.Game.States;

    using SpaceFighter.Logic;
    using SpaceFighter.Logic.Services.Interfaces;

    public class GameplayGameState : GameState
    {
        private readonly Game game;

        readonly ITerrainService terrainService;
        readonly IHeadUpDisplayService headUpDisplayService;
        readonly IAudioService audioService;
        readonly IPlayerService playerService;
        readonly IInputService inputService;
        readonly IEnemyService enemyService;
        readonly ICollisionDetectionService collisionDetectionService;
        readonly ICameraService cameraService;
        readonly IDebugService debugService;
        readonly IGameController gameController;

        public GameplayGameState(
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
        }

        protected override void OnEntered()
        {
            this.game.Components.Add(new FramerateCounter(this.game));

            this.game.Components.Add(this.terrainService);
            this.game.Components.Add(this.headUpDisplayService);
            this.game.Components.Add(this.audioService);
            this.game.Components.Add(this.playerService);
            this.game.Components.Add(this.inputService);
            this.game.Components.Add(this.enemyService);
            this.game.Components.Add(this.collisionDetectionService);
            this.game.Components.Add(this.cameraService);
            this.game.Components.Add(this.debugService);
            this.game.Components.Add(this.gameController);

            this.gameController.StartGame();
            
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.gameController.EndGame();
            this.game.Components.Clear();
            base.OnLeaving();
        }

        protected override void OnPause()
        {

            base.OnPause();
        }

        protected override void OnResume()
        {

            base.OnResume();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {
            // Components do their own updating...
        }
    }
}
