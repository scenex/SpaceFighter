﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class ShotA : IShot
    {
        public ShotA(Vector2 startPosition, int width, int height, Color[] colorData, int firePower, float rotation)
        {           
            this.Position = startPosition;
            this.Width = width;
            this.Height = height;
            this.ColorData = colorData;
            this.FirePower = firePower;
            this.Rotation = rotation;
        }       

        public Color[] ColorData { get; private set; }
        
        public int Width { get; private set; }

        public int Height { get; private set; }

        public Vector2 Position { get; set; }

        public int FirePower { get; private set; }

        public float Rotation { get; private set; }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(
                    (int)this.Position.X,
                    (int)this.Position.Y,
                    this.Width,
                    this.Height);
            }
        }
    }
}
