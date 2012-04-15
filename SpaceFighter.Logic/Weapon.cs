using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Weapon : DrawableGameComponent, IWeapon
    {
        private readonly Texture2D shotSprite;

        private List<Vector2> shotPositionsList;

        private SpriteBatch spriteBatch;

        public Weapon(Game game) : base(game)
        {
            this.shotPositionsList = new List<Vector2>();
            this.shotSprite = this.Game.Content.Load<Texture2D>("Sprites/Spaceship_Shot");
        }

        public void FireWeapon(Vector2 startPosition)
        {
            this.shotPositionsList.Add(new Vector2(
                startPosition.X - ((float)this.shotSprite.Width / 2),
                startPosition.Y - ((float)this.shotSprite.Width / 2)));
        }

        public Vector2[] ShotPositions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < this.shotPositionsList.Count; i++)
            {
                this.shotPositionsList[i] = new Vector2(this.shotPositionsList[i].X, this.shotPositionsList[i].Y - 5);
            }

            // Todo: Remove shot from list when out of window boundaries.
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            foreach (var shotPosition in this.shotPositionsList)
            {
                this.spriteBatch.Draw(shotSprite, shotPosition, Color.White);
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
