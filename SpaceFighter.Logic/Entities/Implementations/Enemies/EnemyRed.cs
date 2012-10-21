﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Interfaces;

    /// <summary>
    /// The enemy class.
    /// </summary>
    public class EnemyRed : DrawableGameComponent, IEnemy
    {
        private readonly Vector2 position;
        private Texture2D sprite;
        private SpriteBatch spriteBatch;
        private Color[] colorData;
        private int energy = 100;

        private Queue<int> shotTriggers;

        public EnemyRed(Game game, Vector2 startPosition) : base(game)
        {
            this.SpawnTimestamp = DateTime.Now;
            this.shotTriggers = new Queue<int>(new List<int>(){2,4,6,8,10});
            this.position = startPosition;
            this.Game.Components.Add(this);
        }

        public int Energy
        {
            get
            {
                return this.energy;
            }
            set
            {
                this.energy = value;
            }
        }

        public Queue<int> ShotTriggers
        {
            get
            {
                return this.shotTriggers;
            }
        }

        public DateTime SpawnTimestamp { get; private set; }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Color[] ColorData
        {
            get
            {
                return this.colorData;
            }
        }

        public int Width
        {
            get
            {
                return this.sprite.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.sprite.Height;
            }
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.sprite = this.Game.Content.Load<Texture2D>("Sprites/Enemy");

            // Obtain color information for subsequent per pixel collision detection
            this.colorData = new Color[this.sprite.Width * this.sprite.Height];
            this.sprite.GetData(this.colorData);

            base.LoadContent();
        }

        /// <summary>
        /// Called when the DrawableGameComponent needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.sprite, this.position, Color.Red);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
