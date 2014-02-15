// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Enemies;
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyFactory : IEnemyFactory
    {
        private readonly Game game;

        private readonly IPathFindingService pathFindingService;

        private readonly ITerrainService terrainService;

        public EnemyFactory(Game game, ITerrainService terrainService)
        {
            this.game = game;
            this.terrainService = terrainService;
            
            this.pathFindingService = new PathFindingService(
                this.terrainService.Map,
                this.terrainService.TileSize,
                this.terrainService.HorizontalTileCount,
                this.terrainService.VerticalTileCount);
        }

        public T CreateAutonomous<T>(Vector2 startPosition, bool isBoss) where T : EnemyAutonomous
        {
            return (T)Activator.CreateInstance(typeof(T), game, pathFindingService, startPosition, isBoss);
        }

        public T CreateScripted<T>(List<Vector2> waypoints, bool isBoss) where T : EnemyScripted
        {
            return (T)Activator.CreateInstance(typeof(T), game, waypoints, isBoss);
        }
    }
}
