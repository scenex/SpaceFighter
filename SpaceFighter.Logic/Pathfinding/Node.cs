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
        public int Parent { get; set; }

        public int F { get; set; }
        public int G { get; set; }
        public int H { get; set; }
    }
}