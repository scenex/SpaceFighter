// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming

namespace SpaceFighter.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using SpaceFighter.Logic.Pathfinding;
    using Xunit;
    using Xunit.Extensions;

    public class AStarTests
    {
        private readonly int[,] tileMap;
        private readonly AStar testee;

        private readonly Node source = new Node(1);
        private readonly Node target = new Node(2);

        public AStarTests()
        {
            // 0x00 -> Walkable
            // 0x01 -> Blocking
            this.tileMap = new[,]
                {
                    { 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00 }
                };

            this.testee = new AStar(source, target, this.tileMap);
        }

        [Theory,
        InlineData(1, new[] { 2, 6, 7 }),
        InlineData(2, new[] { 1, 3, 6, 7, 8 }),
        InlineData(3, new[] { 2, 4, 7, 8, 9 }),
        InlineData(4, new[] { 3, 5, 8, 9, 10 }),
        InlineData(5, new[] { 4, 9, 10 }),
        InlineData(6, new[] { 1, 2, 7, 11, 12 }),
        InlineData(7, new[] { 1, 2, 3, 6, 8, 11, 12, 13 }),
        InlineData(8, new[] { 2, 3, 4, 7, 9, 12, 13, 14 }),
        InlineData(9, new[] { 3, 4, 5, 8, 10, 13, 14, 15 }),
        InlineData(10, new[] { 4, 5, 9, 14, 15 }),
        InlineData(11, new[] { 6, 7, 12, 16, 17 }),
        InlineData(12, new[] { 6, 7, 8, 11, 13, 16, 17, 18 }),
        InlineData(13, new[] { 7, 8, 9, 12, 14, 17, 18, 19 }),
        InlineData(14, new[] { 8, 9, 10, 13, 15, 18, 19, 20 }),
        InlineData(15, new[] { 9, 10, 14, 19, 20 }),
        InlineData(16, new[] { 11, 12, 17, 21, 22 }),
        InlineData(17, new[] { 11, 12, 13, 16, 18, 21, 22, 23 }),
        InlineData(18, new[] { 12, 13, 14, 17, 19, 22, 23, 24 }),
        InlineData(19, new[] { 13, 14, 15, 18, 20, 23, 24, 25 }),
        InlineData(20, new[] { 14, 15, 19, 24, 25 }),
        InlineData(21, new[] { 16, 17, 22 }),
        InlineData(22, new[] { 16, 17, 18, 21, 23 }),
        InlineData(23, new[] { 17, 18, 19, 22, 24 }),
        InlineData(24, new[] { 18, 19, 20, 23, 25 }),
        InlineData(25, new[] { 19, 20, 24 }),]
        public void GetAdjacentNodes_WhenRequested_ThenCorrectlyComputedReturned(int src, int[] nodes)
        {
            var actual = this.testee.GetAdjacentNodes(src);
            actual.Should().OnlyContain(item => nodes.ToList().Contains(item));
        }

        [Fact]
        public void AStar_WhenSetCurrentNode_ThenPutItIntoClosedList()
        {
            var node = new Node(2);
            this.testee.SetCurrentNode(node);
            this.testee.ClosedList.Should().Contain(n => n.Position == node.Position);
        }

        [Fact]
        public void AStar_WhenSetAdjacentNodes_ThenAddToOpenList()
        {
            var nodes = new List<Node> { new Node(2), new Node(3) };

            this.testee.SetAdjacentNodes(source, nodes);
            this.testee.OpenList.Should().Contain(nodes);
        }

        [Fact]
        public void AStar_WhenSetAdjacentNodes_ThenAddToOpenListUniquely()
        {
            var nodes = new List<Node> { new Node(2), new Node(2) };

            this.testee.SetAdjacentNodes(source, nodes);
            this.testee.OpenList.Should().ContainSingle(node => node.Position == 2);
        }

        [Fact]
        public void AStar_WhenSetAdjacentNodes_ThenSetStartingPointAsParentInEachNode()
        {
            var nodes = new List<Node> { new Node(2), new Node(3), new Node(4) };
            this.testee.SetAdjacentNodes(source, nodes);
            this.testee.OpenList.Should().Contain(node => node.Parent == source.Position);
        }

        [Fact]
        public void AStar_WhenComputeOpenListCostH_ThenShouldCorrectlyApplyManhattanMethod()
        {
            var sourceNode = new Node(9);
            var adjacentNodes = new List<Node> { new Node(10) };
            var targetNode = new Node(20);

            this.testee.SetAdjacentNodes(sourceNode, adjacentNodes);
            this.testee.ComputeOpenListCostH(sourceNode, targetNode);

            this.testee.OpenList.Single().H.Should().Be(20);
        }

        [Fact(Skip = "Todo: Implementation")]
        public void AStar_WhenSetAdjacentNodes_ThenCorrectlyCumputeG() // 10 for horizontal-vertical | 14 for diagonal.
        {
            
        }

        [Fact(Skip = "Todo: Implementation")]
        public void AStar_WhenSetAdjacentNodes_ThenCorrectlyCumputeH() // Distance to target, 10 each node.
        {
        }

        [Fact(Skip = "Todo: Implementation")]
        public void AStar_WhenSetAdjacentNodes_ThenCorrectlyCumputeF() // F = G + H
        {
        }
    }
}

// ReSharper restore InconsistentNaming
