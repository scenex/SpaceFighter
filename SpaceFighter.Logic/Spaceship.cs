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
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Spaceship : ISpaceship
    {
        private readonly Texture2D spriteRegular;
        private Vector2 position;
        private Game game;

        private const float moveStep = 2.0f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spaceship"/> class.
        /// </summary>
        /// <param name="game">
        /// The game to be passed.
        /// </param>
        public Spaceship(Game game)
        {
            this.game = game;
            this.position = new Vector2();
            this.spriteRegular = game.Content.Load<Texture2D>("Sprites/Spaceship");
        }

        public event EventHandler CollisionDetected;

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Texture2D SpriteRegular
        {
            get
            {
                return this.spriteRegular;
            }
        }

        public Texture2D SpriteExplosionSequence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IWeapon Weapon
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
    }
}
