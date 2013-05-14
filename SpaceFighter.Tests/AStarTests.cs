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

            this.testee = new AStar(this.tileMap);
        }

        [Theory,
        InlineData(0, new[] { 1, 5, 6 }),
        InlineData(1, new[] { 0, 2, 5, 6, 7 }),
        InlineData(2, new[] { 1, 3, 6, 7, 8 }),
        InlineData(3, new[] { 2, 4, 7, 8, 9 }),
        InlineData(4, new[] { 3, 8, 9 }),
        InlineData(5, new[] { 0, 1, 6, 10, 11 }),
        InlineData(6, new[] { 0, 1, 2, 5, 7, 10, 11, 12 }),
        InlineData(7, new[] { 1, 2, 3, 6, 8, 11, 12, 13 }),
        InlineData(8, new[] { 2, 3, 4, 7, 9, 12, 13, 14 }),
        InlineData(9, new[] { 3, 4, 8, 13, 14 }),
        InlineData(10, new[] { 5, 6, 11, 15, 16 }),
        InlineData(11, new[] { 5, 6, 7, 10, 12, 15, 16, 17 }),
        InlineData(12, new[] { 6, 7, 8, 11, 13, 16, 17, 18 }),
        InlineData(13, new[] { 7, 8, 9, 12, 14, 17, 18, 19 }),
        InlineData(14, new[] { 8, 9, 13, 18, 19 }),
        InlineData(15, new[] { 10, 11, 16, 20, 21 }),
        InlineData(16, new[] { 10, 11, 12, 15, 17, 20, 21, 22 }),
        InlineData(17, new[] { 11, 12, 13, 16, 18, 21, 22, 23 }),
        InlineData(18, new[] { 12, 13, 14, 17, 19, 22, 23, 24 }),
        InlineData(19, new[] { 13, 14, 18, 23, 24 }),
        InlineData(20, new[] { 15, 16, 21 }),
        InlineData(21, new[] { 15, 16, 17, 20, 22 }),
        InlineData(22, new[] { 16, 17, 18, 21, 23 }),
        InlineData(23, new[] { 17, 18, 19, 22, 24 }),
        InlineData(24, new[] { 18, 19, 23 })]
        public void GetAdjacentNodes_WhenRequested_ThenCorrectlyComputedReturned(int nodePosition, int[] nodes)
        {
            var actual = this.testee.GetAdjacentNodes(new Node(nodePosition));
            actual.Should().OnlyContain(item => nodes.ToList().Contains(item.Position));
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
            this.testee.OpenList.Should().Contain(node => node.Parent == source);
        }

        [Fact]
        public void AStar_WhenComputeOpenListCostH_ThenShouldCorrectlyApplyManhattanMethod()
        {
            var sourceNode = new Node(5);
            var adjacentNodes = new List<Node> { new Node(10) };
            var targetNode = new Node(21);

            this.testee.SetAdjacentNodes(sourceNode, adjacentNodes);
            this.testee.ComputeOpenListCostH(targetNode);

            this.testee.OpenList.Single().H.Should().Be(30);
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
