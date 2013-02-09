// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    public static class Vector2Extensions
    {
        public static Vector2 Truncate(this Vector2 vector2, float amount)
        {
            vector2.Normalize();
            return Vector2.Multiply(vector2, amount);
        }
    }
}