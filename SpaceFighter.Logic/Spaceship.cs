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
        private readonly Vector2 position;
        private Game game;    

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
    }
}
