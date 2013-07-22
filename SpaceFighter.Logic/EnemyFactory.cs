// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Enemies;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyFactory : IEnemyFactory
    {
        private readonly Game game;

        private readonly ICameraService cameraService;

        private readonly IPathFindingService pathFindingService;

        private readonly ITerrainService terrainService;

        public EnemyFactory(Game game, ICameraService cameraService, ITerrainService terrainService)
        {
            this.game = game;
            this.cameraService = cameraService;
            this.terrainService = terrainService;
            
            this.pathFindingService = new PathFindingService(
                this.terrainService.Map,
                this.terrainService.TileSize,
                this.terrainService.HorizontalTileCount,
                this.terrainService.VerticalTileCount);
        }

        public T Create<T>(Vector2 startPosition) where T : EnemyBase
        {
            return (T)Activator.CreateInstance(typeof(T), game, cameraService, pathFindingService, startPosition);
        }
    }
}
