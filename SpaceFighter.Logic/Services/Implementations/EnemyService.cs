// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Enemies;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyService : GameComponent, IEnemyService
    {
        private readonly ICameraService cameraService;

        private ITerrainService terrainService;

        public EnemyService(Game game, ICameraService cameraService, ITerrainService terrainService) : base(game)
        {
            this.cameraService = cameraService;
            this.terrainService = terrainService;
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return new List<IEnemy>(Game.Components.OfType<IEnemy>());
            }
        }

        public IEnumerable<IShot> Shots
        {
            get
            {
                return this.Game.Components.OfType<IEnemy>().SelectMany(enemy => enemy.Weapon.Shots);
            }
        }

        public override void Initialize()
        {
            IPathFindingService pathFindingService = new PathFindingService(
                this.terrainService.Map,
                this.terrainService.TileSize,
                this.terrainService.HorizontalTileCount,
                this.terrainService.VerticalTileCount);

            EnemyFactory.Create<EnemyA>(this.Game, this.cameraService, pathFindingService, new Vector2(400, 400)); // Todo: Move TerrainService and CameraService into factory.
            base.Initialize();
        }

        public void ReportEnemyHit(IEnemy enemy, IShot shot)
        {
            enemy.SubtractHealth(shot.FirePower);
        }

        public void RemoveShot(IShot shot)
        {
            var enemies = this.Game.Components.OfType<IEnemy>();
            foreach (var enemy in enemies.Where(enemy => enemy.Weapon.Shots.Contains(shot)))
            {
                enemy.Weapon.Shots.Remove(shot);
            }
        }
    }
}
