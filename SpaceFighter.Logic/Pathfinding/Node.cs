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
        public bool Walkable { get; set; }

        public int G { get; set; }
        public int H { get; set; }

        public int F
        {
            get
            {
                return G + H;
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

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Index == this.Index && other.Position.Equals(this.Position) && Equals(other.Parent, this.Parent) && other.Walkable.Equals(this.Walkable) && other.G == this.G && other.H == this.H;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(Node))
            {
                return false;
            }

            return Equals((Node)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.Index;
                result = (result * 397) ^ this.Position.GetHashCode();
                result = (result * 397) ^ (this.Parent != null ? this.Parent.GetHashCode() : 0);
                result = (result * 397) ^ this.Walkable.GetHashCode();
                result = (result * 397) ^ this.G;
                result = (result * 397) ^ this.H;
                return result;
            }
        }
    }
}