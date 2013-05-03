// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Behaviours
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Global;

    public class BehaviourStrategyPathfinding : IBehaviourStrategy
    {
        private int[,] worldMap;
        private readonly int tileSize;

        private int sourceTile;
        private int destinationTile;

        public BehaviourStrategyPathfinding()
        {
            worldMap = WorldMap.Map;
            tileSize = WorldMap.TileSize;
        }

        public Vector2 Execute(Vector2 source, Vector2 target)
        {
            this.sourceTile = ((int)source.X / tileSize) + ((int)source.Y / tileSize) * (worldMap.GetUpperBound(1) + 1);
            this.destinationTile = ((int)target.X / tileSize) + ((int)target.Y / tileSize) * (worldMap.GetUpperBound(1) + 1);

            return source;
        }
    }
}
