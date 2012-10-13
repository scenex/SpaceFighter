// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IEnemy
    {
        int Energy { get; }
        Vector2 Position { get; }
        Texture2D Sprite { get; }
        Color[] SpriteDataCached { get; }
    }
}
