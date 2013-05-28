// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    // ReSharper disable InconsistentNaming

    public class AStar
    {
        private readonly List<Node> Nodes;

        private readonly int tileSize;
        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;

        private readonly List<Node> openList = new List<Node>();
        private readonly List<Node> closedList = new List<Node>();

        readonly Queue<Vector2> path = new Queue<Vector2>();
        private bool IsPathFound;

        public AStar(int[,] levelMap, int tileSize)
        {
            this.tileSize = tileSize;
            this.Nodes = new List<Node>();

            verticalTileCount = levelMap.GetUpperBound(0) + 1;
            horizontalTileCount = levelMap.GetUpperBound(1) + 1;

            for (var i = 0; i < levelMap.Length; i++)
            {
                var node = new Node(i) { Position = this.IndexToCenterPosition(i) };
                
                if (levelMap[i / this.horizontalTileCount, i % this.horizontalTileCount] == 0)
                {
                    node.Walkable = true;
                }

                this.Nodes.Add(node);
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

        public Queue<Vector2> SolvePath(Vector2 startPosition, int endTileIndex)
        {
            var startNode = this.Nodes.Single(node => node.Index == this.PositionToIndex(startPosition));
            var endNode = this.Nodes.Single(node => node.Index == endTileIndex);

            this.path.Clear();
            this.IsPathFound = false;

            var result = this.SolvePathCore(startNode, endNode);

            this.ReconstructPath(result);

            return new Queue<Vector2>(this.path.Reverse());
        }

        private Node SolvePathCore(Node startNode, Node endNode)
        {
            this.openList.Clear();
            this.closedList.Clear();

            startNode.Parent = null;
            this.openList.Add(startNode);

            startNode.G = 0;
            startNode.H = this.ComputeCostH(startNode, endNode);

            while (this.openList.Count > 0)
            {
                var current = this.openList.OrderBy(node => node.F).First();

                if (current == endNode)
                {
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

        private void ReconstructPath(Node node)
        {
            var current = node.Parent;

            while (current != null && !IsPathFound)
            {
                this.path.Enqueue(current.Position);
                this.ReconstructPath(current);
            }

            IsPathFound = true;
        }

        private IEnumerable<Node> GetNeighbourNodes(Node current)
        {
            var nodeIndices = new List<int>
                {
                    this.GetNodeIndexNW(current.Index),
                    this.GetNodeIndexN(current.Index),
                    this.GetNodeIndexNE(current.Index),
                    this.GetNodeIndexE(current.Index),
                    this.GetNodeIndexSE(current.Index),
                    this.GetNodeIndexS(current.Index),
                    this.GetNodeIndexSW(current.Index),
                    this.GetNodeIndexW(current.Index)
                };

            // Native List<T>.RemoveAll() not supported on Xbox360
            nodeIndices.RemoveAll2(nodeIndex => nodeIndex == -1);

            var adjacentNodes = this.Nodes.Where(node => nodeIndices.Contains(node.Index) && node.Walkable);

            return adjacentNodes.ToList();
        }

        private int ComputeCostG(Node sourceNode, Node targetNode)
        {
            // Vertical or horizontal -> Cost 10
            if (sourceNode.Index == targetNode.Index + 1 ||
                sourceNode.Index == targetNode.Index - 1 ||
                sourceNode.Index == targetNode.Index + horizontalTileCount ||
                sourceNode.Index == targetNode.Index - horizontalTileCount)
            {
                return 10;
            }

            // Diagonal -> Cost 14
            return 14;
        }

        private int ComputeCostH(Node sourceNode, Node targetNode)
        {
            var targetX = targetNode.Index % horizontalTileCount;
            var targetY = targetNode.Index / horizontalTileCount;

            var sourceX = sourceNode.Index % horizontalTileCount;
            var sourceY = sourceNode.Index / horizontalTileCount;

            // Manhattan method
            return 10*(Math.Abs(targetX - sourceX) + Math.Abs(targetY - sourceY));
        }

        private int GetNodeIndexNW(int index)
        {
            if (index - horizontalTileCount - 1 < 0 || index % horizontalTileCount - 1 < 0)
            {
                return -1;
            }

            return index - horizontalTileCount - 1;
        }

        private int GetNodeIndexN(int index)
        {
            if (index - horizontalTileCount < 0)
            {
                return -1;
            }

            return index - horizontalTileCount;
        }

        private int GetNodeIndexNE(int index)
        {
            if (index % horizontalTileCount == horizontalTileCount - 1 || index - horizontalTileCount + 1 < 0)
            {
                return -1;
            }

            return index - horizontalTileCount + 1;
        }

        private int GetNodeIndexE(int index)
        {
            if ((index + 1) % horizontalTileCount == 0)
            {
                return -1;
            }

            return index + 1;
        }

        private int GetNodeIndexSE(int index)
        {
            if ((index + horizontalTileCount + 1) % horizontalTileCount == 0 || index + horizontalTileCount + 1 > horizontalTileCount * verticalTileCount)
            {
                return -1;
            }

            return index + horizontalTileCount + 1;
        }

        private int GetNodeIndexS(int index)
        {
            if (index + horizontalTileCount > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return index + horizontalTileCount;
        }

        private int GetNodeIndexSW(int index)
        {
            if ((index + horizontalTileCount - 1) % horizontalTileCount == horizontalTileCount - 1 || index + horizontalTileCount - 1 > horizontalTileCount * verticalTileCount - 1)
            {
                return -1;
            }

            return index + horizontalTileCount - 1;
        }

        private int GetNodeIndexW(int index)
        {
            if (index % horizontalTileCount == 0)
            {
                return -1;
            }

            return index - 1;
        }

        private Vector2 IndexToCenterPosition(int index)
        {
            return new Vector2(
                (index % this.horizontalTileCount) * this.tileSize + (this.tileSize / 2),
                (index / this.horizontalTileCount) * this.tileSize + (this.tileSize / 2));
        }

        private int PositionToIndex(Vector2 position)
        {
            var x = (int)position.X / this.tileSize;
            var y = (int)position.Y / this.tileSize;

            return x + y * this.horizontalTileCount;
        }
    }

    // ReSharper restore InconsistentNaming
}
