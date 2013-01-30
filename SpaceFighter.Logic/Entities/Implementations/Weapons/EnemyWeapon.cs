// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyWeapon : Weapon
    {
        private readonly IList<IShot> shots;

        private ICameraService cameraService;

        public EnemyWeapon(Game game) : base(game)
        {
            this.shots = new List<IShot>();
        }

        public override void FireWeapon(Vector2 startPosition, int offset, double angle)
        {
            this.shots.Add(
                new Shot(
                    new Vector2(
                        startPosition.X - (this.sprite.Width / 2.0f) + offset * ((float)Math.Cos(angle)),   // Center shot and then add r*cos(angle)
                        startPosition.Y - (this.sprite.Height / 2.0f) + offset * ((float)Math.Sin(angle))),  // Center shot and then add r*sin(angle)

                    this.sprite.Width,
                    this.sprite.Height,
                    this.spriteDataCached,
                    25,
                    angle));
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        public override void LoadShots(string texturePath)
        {
            base.LoadShots("Sprites/Shot");
        }

        public override void UpdateShots()
        {
            for (var i = 0; i < this.shots.Count; i++)
            {
                if (this.shots[i].Position.Y >= 0 && this.shots[i].Position.Y <= Game.GraphicsDevice.PresentationParameters.BackBufferHeight) // <- crap..
                {
                    this.shots[i].Position = new Vector2(
                        (this.shots[i].Position.X + ((float)Math.Cos(this.shots[i].Angle))),
                        (this.shots[i].Position.Y + ((float)Math.Sin(this.shots[i].Angle))));
                }
                else
                {
                    // Remove shots which are not visible anymore.
                    this.shots.Remove(this.shots[i]);
                }
            }
        }

        public override void DrawShots()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            foreach (var shot in this.shots)
            {
                this.spriteBatch.Draw(this.sprite, shot.Position, Color.White);
            }

            this.spriteBatch.End();
        }

        public override IList<IShot> Shots
        {
            get
            {
                return this.shots;
            }
        }
    }
}
