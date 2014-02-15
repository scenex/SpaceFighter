// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities
{
    using Microsoft.Xna.Framework;

    public class Star
    {
        public Star(Vector2 position, int size)
        {
            this.Position = position;
            this.Size = size;
        }

        public Vector2 Position {get; set;}
        
        public int Size { get; set; }

        public int Velocity
        {
            get
            {
                if (this.Size == 1)
                {
                    return 3;
                }

                if (this.Size == 2)
                {
                    return 2;
                }

                if (this.Size == 3)
                {
                    return 1;
                }

                return 1;
            }
        }
    }
}
