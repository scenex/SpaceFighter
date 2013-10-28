﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class CameraService : GameComponent, ICameraService
    {
        private readonly Vector3 screenCenter;

        protected float zoom; 
        public Matrix transform; 
        public Vector2 position;
        protected float rotation;
        const int LevelWidthShift = (1280 - 960) / 2; // Todo: Get from TerrainService

        public CameraService(Game game, Vector3 screenCenter) : base(game)
        {
            this.screenCenter = screenCenter;
            this.zoom = 1.0f;
            this.rotation = 0.0f;
            this.position = Vector2.Zero;
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            this.position += amount;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public Matrix GetTransformation()
        {
            //return Matrix.CreateTranslation(
            //            new Vector3(-this.position.X, -this.position.Y, 0)) *
            //                Matrix.CreateRotationZ(Rotation) *
            //                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
            //                Matrix.CreateTranslation(this.screenCenter);

            return Matrix.CreateTranslation(LevelWidthShift, 0, 1);
        }
    }
}
