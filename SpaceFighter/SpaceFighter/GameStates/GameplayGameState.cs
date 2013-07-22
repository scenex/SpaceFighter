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

        ITerrainService terrainService;
        IHeadUpDisplayService headUpDisplayService;
        IAudioService audioService;
        IPlayerService playerService;
        IInputService inputService;
        IEnemyService enemyService;
        ICollisionDetectionService collisionDetectionService;
        ICameraService cameraService;
        IDebugService debugService;
        IGameController gameController;

        public GameplayGameState(Game game)
        {
            this.game = game;
        }

        protected override void OnEntered()
        {
            this.game.Components.Add(new FramerateCounter(this.game));

            this.terrainService = this.game.Services.GetService(typeof(ITerrainService)) as ITerrainService;
            this.game.Components.Add(this.terrainService);

            this.headUpDisplayService = this.game.Services.GetService(typeof(IHeadUpDisplayService)) as IHeadUpDisplayService;
            this.game.Components.Add(this.headUpDisplayService);

            this.audioService = this.game.Services.GetService(typeof(IAudioService)) as IAudioService;
            this.game.Components.Add(this.audioService);

            this.playerService = this.game.Services.GetService(typeof(IPlayerService)) as IPlayerService;
            this.game.Components.Add(this.playerService);

            this.inputService = this.game.Services.GetService(typeof(IInputService)) as IInputService;
            this.game.Components.Add(this.inputService);

            this.enemyService = this.game.Services.GetService(typeof(IEnemyService)) as IEnemyService;
            this.game.Components.Add(this.enemyService);

            this.collisionDetectionService = this.game.Services.GetService(typeof(ICollisionDetectionService)) as ICollisionDetectionService;
            this.game.Components.Add(this.collisionDetectionService);

            this.cameraService = this.game.Services.GetService(typeof(ICameraService)) as ICameraService;
            this.game.Components.Add(this.cameraService);

            this.debugService = this.game.Services.GetService(typeof(IDebugService)) as IDebugService;
            this.game.Components.Add(this.debugService);

            this.gameController = this.game.Services.GetService(typeof(IGameController)) as IGameController;
            this.game.Components.Add(this.gameController);

            this.gameController.StartGame();
            
            base.OnEntered();
        }

        protected override void OnLeaving()
        {
            this.game.Components.Clear();
            base.OnLeaving();
        }

        /// <summary>
        /// Called when the component needs to update its state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
        public override void Update(GameTime gameTime)
        {

        }
    }
}
