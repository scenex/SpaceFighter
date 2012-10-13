// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IWeapon
    {
        void FireWeapon(Vector2 startPosition);
        IEnumerable<Vector2> SpritePositions { get; }
        Texture2D Sprite { get; }
        Color[] SpriteDataCached { get; }
        int FirePower { get; }
    }
}
