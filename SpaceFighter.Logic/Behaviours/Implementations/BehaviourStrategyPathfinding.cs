// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class BehaviourStrategyPathfinding : BehaviourStrategy
    {
        private readonly int[,] map;
        private readonly int tileSize;

        private int sourceTile;
        private int destinationTile;

        public BehaviourStrategyPathfinding(IWorldService worldService) : base(worldService)
        {
            this.map = this.WorldService.Map;
            this.tileSize = this.WorldService.TileSize;
        }

        public override Vector2 Execute(Vector2 source, Vector2 target)
        {
            this.sourceTile = ((int)source.X / tileSize) + ((int)source.Y / tileSize) * (this.map.GetUpperBound(1) + 1);
            this.destinationTile = ((int)target.X / tileSize) + ((int)target.Y / tileSize) * (this.map.GetUpperBound(1) + 1);

            return source;
        }
    }
}
