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

    public class PlayerWeapon : Weapon
    {
        private readonly IList<IShot> shots;

        private ICameraService cameraService;

        public PlayerWeapon(Game game) : base(game)
        {
            this.shots = new List<IShot>();
        }

        public override void FireWeapon(Vector2 startPosition, int offset, double angle)
        {
            this.shots.Add(
                new Shot(
                     new Vector2(
                        startPosition.X - (this.spriteShot.Width / 2.0f) + offset * ((float)Math.Cos(angle)),   // Center shot and then add r*cos(angle)
                        startPosition.Y - (this.spriteShot.Height / 2.0f) + offset * ((float)Math.Sin(angle))),  // Center shot and then add r*sin(angle)
                    
                    this.spriteShot.Width,
                    this.spriteShot.Height,
                    this.spriteDataCached,
                    20,
                    angle));
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        protected override void LoadShots(string texturePath)
        {
            base.LoadShots("Sprites/Shot");
        }

        protected override void UpdateShots()
        {
            foreach (var shot in this.shots)
            {
                shot.Position = 
                    new Vector2(
                        (shot.Position.X + (float)Math.Cos(shot.Angle) * 5),
                        (shot.Position.Y + (float)Math.Sin(shot.Angle) * 5));
            }
        }

        protected override void DrawShots()
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
                this.spriteBatch.Draw(this.spriteShot, shot.Position, Color.White);
            }

            this.spriteBatch.End();
        }

        protected override void LoadTurret(string texturePath)
        {
            base.LoadTurret("Sprites/Turrets/Turret");
        }

        protected override void UpdateTurret()
        {
            // Do some rotational math calc here...
        }

        protected override void DrawTurret()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

                // Todo: Retrieve turret position
                //this.spriteBatch.Draw(this.spriteTurret, shot.Position, Color.White);
            

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
