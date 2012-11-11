﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class EnemyWeapon : Weapon
    {
        private readonly IList<IShot> shots;

        public EnemyWeapon(Game game) : base(game)
        {
            this.shots = new List<IShot>();
        }

        public override void FireWeapon(Vector2 startPosition, double angle)
        {
            this.shots.Add(
                new Shot(

                    // Todo: Retrieve radius from enemy (radius = enemy origin + enemy length / 2)
                    new Vector2(
                        startPosition.X - (this.sprite.Width / 2.0f)  + 16 * ((float)Math.Cos(angle - MathHelper.PiOver2)),   // Center shot and then add r*cos(angle)
                        startPosition.Y - (this.sprite.Height / 2.0f) + 16 * ((float)Math.Sin(angle - MathHelper.PiOver2))),  // Center shot and then add r*sin(angle)

                    this.sprite.Width,
                    this.sprite.Height,
                    this.spriteDataCached,
                    50,
                    angle));
        }

        public override void LoadShots(string texturePath)
        {
            base.LoadShots("Sprites/Spaceship_Shot");
        }

        public override void UpdateShots()
        {
            for (var i = 0; i < this.shots.Count; i++)
            {
                if (this.shots[i].Position.Y >= 0 && this.shots[i].Position.Y <= Game.GraphicsDevice.PresentationParameters.BackBufferHeight)
                {
                    this.shots[i].Position = new Vector2(
                        (this.shots[i].Position.X + ((float)Math.Cos(this.shots[i].Angle - MathHelper.PiOver2))),
                        (this.shots[i].Position.Y + ((float)Math.Sin(this.shots[i].Angle - MathHelper.PiOver2)))
                        );
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
            this.spriteBatch.Begin();

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
