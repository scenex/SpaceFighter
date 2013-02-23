﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Interfaces;

    public abstract class Weapon : DrawableGameComponent, IWeapon
    {       
        protected SpriteBatch spriteBatch;
        protected Color[] spriteDataCached;
        protected Texture2D spriteShot;
        protected Texture2D spriteTurret;
        protected string path;

        protected Weapon(Game game) : base(game)
        {          
        }

        public abstract IList<IShot> Shots { get; }
        public abstract void FireWeapon(Vector2 startPosition, int offset, double angle);

        protected abstract void DrawShots();
        protected abstract void DrawTurret();

        protected abstract void UpdateShots();
        protected abstract void UpdateTurret();

        protected virtual void LoadTurret(string texturePath)
        {
            this.path = texturePath;
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteTurret = this.Game.Content.Load<Texture2D>(this.path);

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.spriteTurret.Width * this.spriteTurret.Height];
            this.spriteTurret.GetData(this.spriteDataCached);
        }

        protected virtual void LoadShots(string texturePath)
        {
            this.path = texturePath;
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteShot = this.Game.Content.Load<Texture2D>(this.path);

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.spriteShot.Width * this.spriteShot.Height];
            this.spriteShot.GetData(this.spriteDataCached);
        }

        protected override void LoadContent()
        {
            this.LoadShots(this.path);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateShots();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawTurret();
            this.DrawShots();
            base.Draw(gameTime);
        }
    }
}
