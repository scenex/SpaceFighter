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
        protected Texture2D sprite;

        protected Weapon(Game game) : base(game)
        {          
        }
        
        public abstract IList<IShot> Shots { get; }
        public abstract void FireWeapon(Vector2 startPosition);
        public abstract void LoadShots();
        public abstract void UpdateShots();
        public abstract void DrawShots();

        protected override void LoadContent()
        {
            this.LoadShots();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateShots();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawShots();
            base.Draw(gameTime);
        }
    }
}
