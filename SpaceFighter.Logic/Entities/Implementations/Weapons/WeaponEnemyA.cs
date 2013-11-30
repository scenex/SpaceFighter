// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WeaponEnemyA : Weapon
    {
        public WeaponEnemyA(
            Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.LoadShot("Sprites/Shot");
            base.LoadContent();
        }

        public override void FireWeapon()
        {
            const int Offset = 108 / 2;

            this.Shots.Add(
                new ShotA(
                    new Vector2(
                        this.Position.X - (this.SpriteShot.Width / 2.0f) + Offset * ((float)Math.Cos(this.Rotation)),   // Center shot and then add r*cos(angle)
                        this.Position.Y - (this.SpriteShot.Height / 2.0f) + Offset * ((float)Math.Sin(this.Rotation))),  // Center shot and then add r*sin(angle)
                    this.SpriteShot.Width,
                    this.SpriteShot.Height,
                    this.SpriteShotDataCached,
                    25,
                    this.Rotation));
        }

        protected override void UpdateGameTime(GameTime gameTime)
        {
            // Nothing to do here yet.
        }

        protected override void UpdateShots()
        {
            foreach (var shot in this.Shots)
            {
                shot.Position = 
                    new Vector2(
                        (shot.Position.X + (float)Math.Cos(shot.Angle) * 3),
                        (shot.Position.Y + (float)Math.Sin(shot.Angle) * 3));
            }
        }

        protected override void DrawShots()
        {
            this.SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend);

            foreach (var shot in this.Shots)
            {
                this.SpriteBatch.Draw(this.SpriteShot, shot.Position, Color.White);
            }

            this.SpriteBatch.End();
        }
    }
}
