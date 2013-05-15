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

        private List<Node> Nodes; 

        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;

        private readonly List<Node> openList = new List<Node>();
        private readonly List<Node> closedList = new List<Node>();

        public AStar(int[,] levelMap)
        {
            //this.levelMap = levelMap;
            this.Nodes = new List<Node>();

            verticalTileCount = levelMap.GetUpperBound(0) + 1;
            horizontalTileCount = levelMap.GetUpperBound(1) + 1;

            // Create and initialize nodes with their positions
            for (var i = 0; i < levelMap.Length; i++) // TODO: FIX CORRECT LENGTH
            {
                this.Nodes.Add(new Node(i));
            }
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

        // WORK IN PROGRESS
        public void SolvePath(Node startNode, Node endNode)
        {
            this.openList.Clear();
            this.openList.Add(startNode);

            this.closedList.Clear();

            while (!this.Nodes.Contains(endNode) || this.Nodes.Count > 0)
            {
                this.openList.AddRange(this.GetAdjacentNodes(startNode));
                var current = this.openList.First(node => node.F > 0);
                
                this.openList.Remove(current);
                this.closedList.Add(current);

                var currentAdjacentNodes = this.GetAdjacentNodes(current);

                foreach (var adjacentNode in currentAdjacentNodes)
                {
                    if (!this.closedList.Contains(adjacentNode))
                    {
                        if (!this.openList.Contains(adjacentNode))
                        {
                            this.openList.Add(adjacentNode);
                            adjacentNode.Parent = current;

                            adjacentNode.H = this.ComputeCostH(adjacentNode, endNode);
                            adjacentNode.G = this.ComputeCostG(adjacentNode, endNode);
                            adjacentNode.F = this.ComputeCostF(adjacentNode, endNode);
                        }
                    }
                }
            }
        }

        public List<Node> GetAdjacentNodes(Node current)
        {
            var nodePositions = new List<int>
                {
                    this.GetNodePositionNW(current.Position),
                    this.GetNodePositionN(current.Position),
                    this.GetNodePositionNE(current.Position),
                    this.GetNodePositionE(current.Position),
                    this.GetNodePositionSE(current.Position),
                    this.GetNodePositionS(current.Position),
                    this.GetNodePositionSW(current.Position),
                    this.GetNodePositionW(current.Position)
                };

            // Native List<T>.RemoveAll() not supported on Xbox360
            nodePositions.RemoveAll2(nodePosition => nodePosition == -1);

            var adjacentNodes = this.Nodes.Where(node => nodePositions.Contains(node.Position));

            return adjacentNodes.ToList();
        }

        public void SetAdjacentNodes(Node sourceNode, List<Node> adjacentNodes)
        {
            foreach (var node in adjacentNodes)
            {
                node.Parent = sourceNode;
            }

            this.openList.AddRange(adjacentNodes.Distinct(new NodeComparer()));
        }

        public void SetCurrentNode(Node node)
        {
            this.openList.Remove(node);
            this.closedList.Add(node);
        }

        private int ComputeCostF(Node adjacentNode, Node endNode)
        {
            throw new NotImplementedException();
        }

        private int ComputeCostG(Node adjacentNode, Node endNode)
        {
            throw new NotImplementedException();
        }

        public int ComputeCostH(Node sourceNode, Node targetNode)
        {
            var targetX = targetNode.Position % horizontalTileCount;
            var targetY = targetNode.Position / horizontalTileCount;

            var sourceX = sourceNode.Position % horizontalTileCount;
            var sourceY = sourceNode.Position / horizontalTileCount;

            // Manhattan method
            return 10*(Math.Abs(targetX - sourceX) + Math.Abs(targetY - sourceY));
        }

        public int GetNodePositionNW(int position)
        {
            if (position - horizontalTileCount - 1 < 0 || position % horizontalTileCount - 1 < 0)
            {
                return -1;
            }

            return position - horizontalTileCount - 1;
        }

        public int GetNodePositionN(int position)
        {
            if (position - horizontalTileCount < 0)
            {
                return -1;
            }

            return position - horizontalTileCount;
        }

        public int GetNodePositionNE(int position)
        {
            if (position % horizontalTileCount == horizontalTileCount - 1 || position - horizontalTileCount + 1 < 0)
            {
                return -1;
            }

            return position - horizontalTileCount + 1;
        }

        public int GetNodePositionE(int position)
        {
            if ((position + 1) % horizontalTileCount == 0)
            {
                return -1;
            }

            return position + 1;
        }

        public int GetNodePositionSE(int position)
        {
            if ((position + horizontalTileCount + 1) % horizontalTileCount == 0 || position + horizontalTileCount + 1 > horizontalTileCount * verticalTileCount)
            {
                return -1;
            }

            return position + horizontalTileCount + 1;
        }

        public int GetNodePositionS(int position)
        {
            if (position + horizontalTileCount > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return position + horizontalTileCount;
        }

        public int GetNodePositionSW(int position)
        {
            if ((position + horizontalTileCount - 1) % horizontalTileCount == horizontalTileCount - 1 || position + horizontalTileCount - 1 > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return position + horizontalTileCount - 1;
        }

        public int GetNodePositionW(int position)
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
