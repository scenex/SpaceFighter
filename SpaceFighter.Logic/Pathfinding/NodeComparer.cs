// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Pathfinding
{
    using System;
    using System.Collections.Generic;

    public class NodeComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
        {
            return x.Position == y.Position;
        }

        public int GetHashCode(Node obj)
        {
            var hash = obj.Position.GetHashCode();
            return hash;
        }
    }
}