// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Interfaces;

    public class PlayerWeapon : Weapon
    {
        private readonly IList<IShot> shots;

        public PlayerWeapon(Game game) : base(game)
        {
            this.shots = new List<IShot>();
        }

        public override void FireWeapon(Vector2 startPosition)
        {
            this.shots.Add(
                new Shot(
                    new Vector2(startPosition.X - (float)this.sprite.Width / 2, startPosition.Y - (float)this.sprite.Height / 2),
                    this.sprite.Width,
                    this.sprite.Height,
                    this.spriteDataCached,
                    50));
        }

        public override void LoadShots()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.sprite = this.Game.Content.Load<Texture2D>("Sprites/Spaceship_Shot");

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.sprite.Width * this.sprite.Height];
            this.sprite.GetData(this.spriteDataCached);
        }

        public override void UpdateShots()
        {
            for (var i = 0; i < this.shots.Count; i++)
            {
                if (this.shots[i].Position.Y >= 0)
                {
                    this.shots[i].Position = new Vector2(this.shots[i].Position.X, this.shots[i].Position.Y - 5);
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
