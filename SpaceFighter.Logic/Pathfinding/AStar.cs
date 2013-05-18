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
        private readonly List<Node> Nodes; 
        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;
        private readonly List<Node> openList = new List<Node>();
        private readonly List<Node> closedList = new List<Node>();

        public AStar(int[,] levelMap)
        {
            this.Nodes = new List<Node>();

            verticalTileCount = levelMap.GetUpperBound(0) + 1;
            horizontalTileCount = levelMap.GetUpperBound(1) + 1;

            // Create and initialize nodes with their positions (Determining if block passable?)
            for (var i = 0; i < levelMap.Length; i++)
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
        public Node SolvePath(Node startNode, Node endNode)
        {
            this.openList.Clear();
            this.closedList.Clear();
            
            this.openList.Add(startNode);

            startNode.G = 0;
            startNode.H = this.ComputeCostH(startNode, endNode);

            while (this.openList.Count > 0)
            {
                var current = this.openList.OrderBy(node => node.F).First();

                if (current == endNode)
                {
                    // Reconstruct path by traversing parents while != null
                    return current;
                }

                this.openList.Remove(current);
                this.closedList.Add(current);
                   
                foreach (var neighbour in this.GetNeighbourNodes(current))
                {
                    var g_score_temp = current.G + this.ComputeCostG(current, neighbour);
                    
                    if (this.closedList.Contains(neighbour) && g_score_temp < neighbour.G)
                    {
                        neighbour.G = g_score_temp;
                        neighbour.Parent = current;
                    }
                    else if (this.openList.Contains(neighbour) && g_score_temp < neighbour.G)
                    {
                        neighbour.G = g_score_temp;
                        neighbour.Parent = current;
                    }
                    else if (!this.openList.Contains(neighbour) && !this.closedList.Contains(neighbour))
                    {
                        this.openList.Add(neighbour);
                        neighbour.Parent = current;
                        neighbour.G = current.G + this.ComputeCostG(neighbour, current);
                        neighbour.H = this.ComputeCostH(neighbour, endNode);
                    }
                }
            }

            return null;
        }

        public List<Node> GetNeighbourNodes(Node current)
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

        private int ComputeCostG(Node sourceNode, Node targetNode)
        {
            // Vertical or horizontal -> Cost 10
            if (sourceNode.Position == targetNode.Position + 1 ||
                sourceNode.Position == targetNode.Position - 1 ||
                sourceNode.Position == targetNode.Position + horizontalTileCount ||
                sourceNode.Position == targetNode.Position - horizontalTileCount)
            {
                return 10;
            }

            // Diagonal -> Cost 14
            return 14;
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
