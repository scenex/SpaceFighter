// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class Shot : IShot
    {
        public Shot(Vector2 startPosition, int width, int height, Color[] colorData, int firePower)
        {           
            this.Position = startPosition;
            this.Width = width;
            this.Height = height;
            this.ColorData = colorData;
            this.FirePower = firePower;
        }

        public Color[] ColorData { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public Vector2 Position { get; set; }

        public int FirePower { get; private set; }
    }
}
