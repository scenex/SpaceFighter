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

    public abstract class Weapon : DrawableGameComponent, IWeapon
    {
        public SpriteManager SpriteManager;

        protected SpriteBatch SpriteBatch;
        protected Color[] SpriteShotDataCached;
        protected Texture2D SpriteShot;
        protected string PathShot;
        protected int UpgradeLevel = 1;

        protected Weapon(Game game) : base(game)
        {
            this.Shots = new List<IShot>();
        }

        public event EventHandler<EventArgs> WeaponFired;

        public virtual float Rotation { get; set; }
        public virtual Vector2 Position { get; set; }
        
        public abstract void FireWeapon();

        protected abstract void UpdateGameTime(GameTime gameTime);

        protected abstract void DrawShots();
        protected abstract void UpdateShots();      

        protected virtual void DrawTurret() {}
        protected virtual void LoadTurret() {}

        public IList<IShot> Shots { get; protected set; }

        protected virtual void LoadShot(string texturePath)
        {
            this.PathShot = texturePath;
            this.SpriteShot = this.Game.Content.Load<Texture2D>(this.PathShot);

            // Obtain color information for subsequent per pixel collision detection
            this.SpriteShotDataCached = new Color[this.SpriteShot.Width * this.SpriteShot.Height];
            this.SpriteShot.GetData(this.SpriteShotDataCached);
        }

        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);        
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateGameTime(gameTime);
            this.UpdateShots();
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawTurret();
            this.DrawShots();

            base.Draw(gameTime);
        }

        public void UpgradeWeapon()
        {
            // Todo: No Upper limit, just for testing purposes
            if (this.UpgradeLevel < 2) 
            {
                this.UpgradeLevel++;
            }
        }

        public void DowngradeWeapon()
        {
            if (this.UpgradeLevel > 1)
            {
                this.UpgradeLevel--;
            }
        }

        protected void TriggerWeaponFiredEvent(object sender, EventArgs eventArgs)
        {
            if (this.WeaponFired != null)
            {
                this.WeaponFired(this, null);
            }
        }
    }
}
