// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // ReSharper disable InconsistentNaming

    public class AStar
    {
        int[,] levelMap;

        private readonly Node source;
        private readonly Node target;

        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;

        private readonly List<Node> openList = new List<Node>();
        private readonly List<Node> closedList = new List<Node>();

        public AStar(Node source, Node target, int[,] levelMap)
        {
            this.levelMap = levelMap;
            this.source = source;
            this.target = target;

            verticalTileCount = levelMap.GetUpperBound(0) + 1;
            horizontalTileCount = levelMap.GetUpperBound(1) + 1;

            //this.OpenList.Add(source);
        }

        public List<Node> OpenList
        {
            get
            {
                return this.openList;
            }
        }

        public List<Node> ClosedList
        {
            get
            {
                return this.closedList;
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

            // Native List<T>.RemoveAll() not supported on Xbox360
            nodes.RemoveAll2(node => node == -1);

            return nodes.ToArray();
        }

        public void SetAdjacentNodes(Node sourceNode, List<Node> adjacentNodes)
        {
            foreach (var node in adjacentNodes)
            {
                node.Parent = sourceNode.Position;
            }

            this.openList.AddRange(adjacentNodes.Distinct(new NodeComparer()));
        }

        public void SetCurrentNode(Node node)
        {
            this.openList.Remove(node);
            this.closedList.Add(node);
        }

        public void ComputeOpenListCostH(Node targetNode) // Manhattan method
        {
            var targetX = targetNode.Position % horizontalTileCount;
            var targetY = (targetNode.Position / horizontalTileCount);

            foreach (var node in openList)
            {
                var sourceX = node.Position % horizontalTileCount;
                var sourceY = (node.Position / horizontalTileCount);

                node.H = 10*(Math.Abs(targetX - sourceX) + Math.Abs(targetY - sourceY));
            }
        }

        public int GetNodeNW(int position)
        {
            if (position - horizontalTileCount - 1 < 0 || position % horizontalTileCount - 1 < 0)
            {
                return -1;
            }

            return position - horizontalTileCount - 1;
        }

        public int GetNodeN(int position)
        {
            if (position - horizontalTileCount < 0)
            {
                return -1;
            }

            return position - horizontalTileCount;
        }

        public int GetNodeNE(int position)
        {
            if (position % horizontalTileCount == horizontalTileCount - 1 || position - horizontalTileCount + 1 < 0)
            {
                return -1;
            }

            return position - horizontalTileCount + 1;
        }

        public int GetNodeE(int position)
        {
            if ((position + 1) % horizontalTileCount == 0)
            {
                return -1;
            }

            return position + 1;
        }

        public int GetNodeSE(int position)
        {
            if ((position + horizontalTileCount + 1) % horizontalTileCount == 0 || position + horizontalTileCount + 1 > horizontalTileCount * verticalTileCount)
            {
                return -1;
            }

            return position + horizontalTileCount + 1;
        }

        public int GetNodeS(int position)
        {
            if (position + horizontalTileCount > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return position + horizontalTileCount;
        }

        public int GetNodeSW(int position)
        {
            if ((position + horizontalTileCount - 1) % horizontalTileCount == horizontalTileCount - 1 || position + horizontalTileCount - 1 > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return position + horizontalTileCount - 1;
        }

        public int GetNodeW(int position)
        {
            if (position % horizontalTileCount == 0)
            {
                return -1;
            }

            return position - 1;
        }
    }

    // ReSharper restore InconsistentNaming
}
