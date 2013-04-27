// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System.Collections.Generic;

    // ReSharper disable InconsistentNaming

    public class AStar
    {
        int[,] levelMap;
        int tileSize;

        private readonly int source;
        private readonly int target;

        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;

        private readonly List<int> openList = new List<int>();

        public AStar(int source, int target, int[,] levelMap, int tileSize)
        {
            this.levelMap = levelMap;
            this.tileSize = tileSize;
            this.source = source;
            this.target = target;

            verticalTileCount = levelMap.GetUpperBound(0) + 1;
            horizontalTileCount = levelMap.GetUpperBound(1) + 1;

            this.OpenList.Add(source);
        }

        public List<int> OpenList
        {
            get
            {
                return this.openList;
            }
        }

        public int[] GetAdjacentNodes(int position)
        {
            var nodes = new List<int>
                {
                    this.GetNodeNW(position),
                    this.GetNodeN(position),
                    this.GetNodeNE(position),
                    this.GetNodeE(position),
                    this.GetNodeSE(position),
                    this.GetNodeS(position),
                    this.GetNodeSW(position),
                    this.GetNodeW(position)
                };

            nodes.RemoveAll2(node => node == 0); // Native List<T>.RemoveAll() not supported on Xbox360
            return nodes.ToArray();
        }

        public int GetNodeNW(int position)
        {
            if (position - horizontalTileCount - 1 < 1 || position % horizontalTileCount - 1 < 1)
            {
                return 0;
            }

            return position - horizontalTileCount - 1;
        }

        public int GetNodeN(int position)
        {
            if (position - horizontalTileCount < 1)
            {
                return 0;
            }

            return position - horizontalTileCount;
        }

        public int GetNodeNE(int position)
        {
            if (position % horizontalTileCount == 0 || position - horizontalTileCount + 1 < 1)
            {
                return 0;
            }

            return position - horizontalTileCount + 1;
        }

        public int GetNodeE(int position)
        {
            if (position % horizontalTileCount == 0)
            {
                return 0;
            }

            return position + 1;
        }

        public int GetNodeSE(int position)
        {
            if (position % horizontalTileCount == 0 || position + horizontalTileCount + 1 > horizontalTileCount * verticalTileCount)
            {
                return 0;
            }

            return position + horizontalTileCount + 1;
        }

        public int GetNodeS(int position)
        {
            if (position + horizontalTileCount > horizontalTileCount * verticalTileCount)
            {
                return 0;
            }

            return position + horizontalTileCount;
        }

        public int GetNodeSW(int position)
        {
            if ((position + horizontalTileCount - 1) % horizontalTileCount == 0 || position + horizontalTileCount - 1 > horizontalTileCount * verticalTileCount)
            {
                return 0;
            }

            return position + horizontalTileCount - 1;
        }

        public int GetNodeW(int position)
        {
            if (position % horizontalTileCount == 1)
            {
                return 0;
            }

            return position - 1;
        }
    }

    // ReSharper restore InconsistentNaming
}
