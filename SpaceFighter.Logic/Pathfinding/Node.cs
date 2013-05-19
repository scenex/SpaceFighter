// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Pathfinding
{
    public class Node
    {
        public Node(int position)
        {
            this.Position = position;
        }

        public int Position { get; private set; }
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
            return a.Position == b.Position;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }
    }
}