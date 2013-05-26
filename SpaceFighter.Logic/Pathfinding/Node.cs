// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Pathfinding
{
    using Microsoft.Xna.Framework;

    public class Node
    {
        public Node(int index)
        {
            this.Index = index;
        }

        public int Index { get; private set; }
        public Vector2 Position { get; set; }

        public Node Parent { get; set; }

        public int G { get; set; }
        public int H { get; set; }

        public int F
        {
            get
            {
                return G + H;
            }
        }

        public bool Walkable
        {
            get
            {
                // Todo
                return true;
            }
        }

        public static bool operator ==(Node a, Node b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Index == b.Index;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }
    }
}