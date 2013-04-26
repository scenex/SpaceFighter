// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Behaviours
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Global;

    public class BehaviourStrategyPathfinding : IBehaviourStrategy
    {
        private int[,] worldMap;
        private int tileSize;

        public BehaviourStrategyPathfinding()
        {
            worldMap = WorldMap.Map;
            tileSize = WorldMap.TileSize;
        }

        public Vector2 Execute(Vector2 enemyPosition, Vector2 playerPosition)
        {
            throw new NotImplementedException();
        }
    }
}
