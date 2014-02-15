// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Pathfinding;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PathFindingService : IPathFindingService
    {
        private readonly int[,] map;
        private readonly int tileSize;
        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;

        private readonly Random random = new Random();
        private List<int> collidableTileIndices;
        private List<int> nonCollidableTileIndices;
        private int tileIndexToNavigate;

        private readonly AStar pathfinder;

        public PathFindingService(int[,] map, int tileSize, int horizontalTileCount, int verticalTileCount)
        {
            this.map = map;
            this.tileSize = tileSize;
            this.horizontalTileCount = horizontalTileCount;
            this.verticalTileCount = verticalTileCount;

            this.pathfinder = new AStar(this.map, this.tileSize);
        }

        public List<Vector2> GetPathToRandomTile(Vector2 sourcePosition)
        {
            this.SetRandomTile();
            return this.pathfinder.SolvePath(sourcePosition, this.tileIndexToNavigate).ToList();
        }

        private void SetRandomTile()
        {
            this.tileIndexToNavigate = this.GetNonCollidableTileIndices().ElementAt(this.random.Next(0, this.GetNonCollidableTileIndicesCount() - 1));
        }

        private IEnumerable<int> GetCollidableTileIndices()
        {
            if (this.collidableTileIndices == null)
            {
                this.collidableTileIndices = new List<int>();

                for (var i = 0; i < this.verticalTileCount; i++)
                {
                    for (var j = 0; j < this.horizontalTileCount; j++)
                    {
                        if (this.map[i, j] != 0)
                        {
                            this.collidableTileIndices.Add(i * this.horizontalTileCount + j);
                        }
                    }
                }
            }

            return collidableTileIndices;
        }

        private IEnumerable<int> GetNonCollidableTileIndices()
        {
            if (this.nonCollidableTileIndices == null)
            {
                nonCollidableTileIndices = new List<int>();

                for (var i = 0; i < this.verticalTileCount; i++)
                {
                    for (var j = 0; j < this.horizontalTileCount; j++)
                    {
                        if (this.map[i, j] == 0)
                        {
                            this.nonCollidableTileIndices.Add(i * this.horizontalTileCount + j);
                        }
                    }
                }
            }

            return nonCollidableTileIndices;
        }

        private int GetCollidableTileIndicesCount()
        {
            return this.GetCollidableTileIndices().Count();
        }

        private int GetNonCollidableTileIndicesCount()
        {
            return this.GetNonCollidableTileIndices().Count();
        }
    }
}
