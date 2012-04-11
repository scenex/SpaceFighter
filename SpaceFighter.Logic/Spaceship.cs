// -----------------------------------------------------------------------
// <copyright file="Spaceship.cs" company="Catclysm">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Spaceship : DrawableGameComponent, ISpaceship
    {
        private readonly Texture2D shipRegular;
        
        private Vector2 position;
        private ContentManager contentManager;

        private const float moveStep = 2.0f;

        private Weapon weapon;

        private SpriteBatch spriteBatch;

        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spaceship"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="contentManager">
        /// The game to be passed.
        /// </param>
        /// <param name="startPosition">
        /// The start Position.
        /// </param>
        public Spaceship(Game game, ContentManager contentManager, Vector2 startPosition) : base(game)
        {
            this.game = game;
            this.contentManager = contentManager;
            this.position = startPosition;

            this.shipRegular = this.contentManager.Load<Texture2D>("Sprites/Spaceship");
        }

        public event EventHandler CollisionDetected;

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Texture2D ShipRegular
        {
            get
            {
                return this.shipRegular;
            }
        }

        public Texture2D ShipExplosionSequence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void MoveLeft()
        {
            this.position.X -= moveStep;
        }

        public void MoveRight()
        {
            this.position.X += moveStep;
        }

        public void MoveUp()
        {
            this.position.Y -= moveStep;
        }

        public void MoveDown()
        {
            this.position.Y += moveStep;
        }

        public void UpgradeWeapon()
        {
            throw new NotImplementedException();
        }

        public void DowngradeWeapon()
        {
            throw new NotImplementedException();
        }

        public void FireWeapon()
        {
            this.weapon.FireWeapon(this.position);
        }

        public override void Initialize()
        {
            this.weapon = new Weapon(this.game);
            this.game.Components.Add(this.weapon);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.ShipRegular, this.Position, Color.White);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
