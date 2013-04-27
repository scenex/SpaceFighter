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

        public Vector2 Execute(Vector2 source, Vector2 destination)
        {
            this.sourceTile = ((int)source.X / tileSize) + ((int)source.Y / tileSize) * 17;
            this.destinationTile = ((int)destination.X / tileSize) + ((int)destination.Y / tileSize) * 17;

            return source;
        }
    }
}
