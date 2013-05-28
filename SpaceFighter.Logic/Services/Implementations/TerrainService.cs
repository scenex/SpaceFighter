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

    public class TerrainService : ITerrainService
    {
        private readonly Random random = new Random();
        private List<int> collidableTileIndices;
        private List<int> nonCollidableTileIndices;
        private int tileIndexToNavigate;

        private readonly AStar pathfinder;

        public TerrainService()
        {
            this.TileSize = 80;

            this.Map = new[,]
                {
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x04, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x07, 0x08, 0x08, 0x09, 0x00, 0x05 },
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 }
                };

            this.pathfinder = new AStar(this.Map, this.TileSize);

            this.VerticalTileCount = this.Map.GetUpperBound(0) + 1;
            this.HorizontalTileCount = this.Map.GetUpperBound(1) + 1;
        }

        public int TileSize { get; private set; }
        public int[,] Map { get; private set; }
        public int VerticalTileCount { get; private set; }
        public int HorizontalTileCount { get; private set; }

        public int LevelWidth
        {
            get
            {
                return this.HorizontalTileCount * TileSize;
            }
        }

        public int LevelHeight
        {
            get
            {
                return this.VerticalTileCount * TileSize;
            }
        }

        // IGameComponent
        public void Initialize()
        {
        }

        public Queue<Vector2> GetPathToRandomTile(Vector2 sourcePosition)
        {
            this.SetRandomTile();
            return this.pathfinder.SolvePath(sourcePosition, this.tileIndexToNavigate);
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

                for (var i = 0; i < this.VerticalTileCount; i++)
                {
                    for (var j = 0; j < this.HorizontalTileCount; j++)
                    {
                        if (this.Map[i,j] != 0)
                        {
                            this.collidableTileIndices.Add(i * this.HorizontalTileCount + j);
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

                for (var i = 0; i < this.VerticalTileCount; i++)
                {
                    for (var j = 0; j < this.HorizontalTileCount; j++)
                    {
                        if (this.Map[i, j] == 0)
                        {
                            this.nonCollidableTileIndices.Add(i * this.HorizontalTileCount + j);
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