// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    public static class Vector2Extensions
    {
        public static Vector2 Truncate(this Vector2 vector, float max)
        {
            var i = max / vector.Length();
            i = i < 1.0 ? 1.0f : i;
            return Vector2.Multiply(vector, i);
        }
    }
}