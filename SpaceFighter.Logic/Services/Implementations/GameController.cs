// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class GameController : GameComponent, IGameController
    {
        private CollisionDetectionService collisionDetectionService;
        private PlayerService playerService;
        private EnemyService enemyService;

        public GameController(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.collisionDetectionService = new CollisionDetectionService(this.Game);
            this.Game.Components.Add(this.collisionDetectionService);

            this.playerService = new PlayerService(this.Game);
            this.Game.Services.AddService(typeof(IPlayerService), this.playerService);
            this.Game.Components.Add(this.playerService);

            this.enemyService = new EnemyService(this.Game);
            this.Game.Services.AddService(typeof(IEnemyService), this.enemyService);
            this.Game.Components.Add(this.enemyService);

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            
            base.Initialize();
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs e)
        {
            this.enemyService.RemoveShot(e.Shot);
            this.playerService.ReportPlayerHit(e.Shot);
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.playerService.RemoveShot(e.Shot);
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);         
        }
    }
}
