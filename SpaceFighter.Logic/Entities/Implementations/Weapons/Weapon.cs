// -----------------------------------------------------------------------
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
        protected string pathShot;
        protected string pathTurret;

        protected Weapon(Game game) : base(game)
        {          
        }

        public virtual float Rotation { get; protected set; }
        public virtual Vector2 Position { get; protected set; }

        public abstract IList<IShot> Shots { get; }
        public abstract void FireWeapon(int offset);

        public virtual void SetRotation(float angle) {}
        public virtual void SetPosition(Vector2 pos) {}

        protected abstract void DrawShots();
        protected abstract void UpdateShots();

        protected virtual void DrawTurret() {}
        protected virtual void UpdateTurret() { }

        protected virtual void LoadTurret(string texturePath)
        {
            this.pathTurret = texturePath;            
            this.spriteTurret = this.Game.Content.Load<Texture2D>(this.pathTurret);
        }

        protected virtual void LoadShot(string texturePath)
        {
            this.pathShot = texturePath;
            this.spriteShot = this.Game.Content.Load<Texture2D>(this.pathShot);

            // Obtain color information for subsequent per pixel collision detection
            this.spriteDataCached = new Color[this.spriteShot.Width * this.spriteShot.Height];
            this.spriteShot.GetData(this.spriteDataCached);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);        
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {   
            this.UpdateTurret();
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
