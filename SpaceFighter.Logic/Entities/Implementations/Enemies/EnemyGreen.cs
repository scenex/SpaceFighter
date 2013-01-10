﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// The enemy class.
    /// </summary>
    public class EnemyGreen : DrawableGameComponent, IEnemy
    {
        private Vector2 position;
        private readonly Queue<TimeSpan> weaponTriggers;
        private Texture2D sprite;
        private SpriteBatch spriteBatch;
        private Color[] colorData;
        private int health = 100;
        
        private float rotation;

        private double angleToPlayer;

        private IEnumerable<Vector2> waypoints;

        private readonly Curve enemyCurveX = new Curve();
        private readonly Curve enemyCurveY = new Curve();

        private ICameraService cameraService;

        public EnemyGreen(Game game, IEnumerable<Vector2> waypoints) : base(game)
        {
            this.weaponTriggers = new Queue<TimeSpan>(new List<TimeSpan>(){ 
                new TimeSpan(0,0,0,0),
                new TimeSpan(0,0,0,2),
                new TimeSpan(0,0,0,4),
                new TimeSpan(0,0,0,6),
                new TimeSpan(0,0,0,8),
                new TimeSpan(0,0,0,10),
                new TimeSpan(0,0,0,12),
                new TimeSpan(0,0,0,14),
                new TimeSpan(0,0,0,16),
                new TimeSpan(0,0,0,18),
                new TimeSpan(0,0,0,20),
                new TimeSpan(0,0,0,22),
                new TimeSpan(0,0,0,24),
                new TimeSpan(0,0,0,26),
                new TimeSpan(0,0,0,28),
                new TimeSpan(0,0,0,30),
                new TimeSpan(0,0,0,32),
                new TimeSpan(0,0,0,34),});

            this.enemyCurveX.Keys.Add(new CurveKey(0.0f, 200));
            this.enemyCurveX.Keys.Add(new CurveKey(4.0f, 300));
            this.enemyCurveX.Keys.Add(new CurveKey(8.0f, 200));
            this.enemyCurveX.Keys.Add(new CurveKey(12.0f,100));
            this.enemyCurveX.Keys.Add(new CurveKey(16.0f,200));

            this.enemyCurveY.Keys.Add(new CurveKey(0.0f, 200));
            this.enemyCurveY.Keys.Add(new CurveKey(4.0f, 300));
            this.enemyCurveY.Keys.Add(new CurveKey(8.0f, 400));
            this.enemyCurveY.Keys.Add(new CurveKey(12.0f,300));
            this.enemyCurveY.Keys.Add(new CurveKey(16.0f,200));

            this.position = waypoints.First();
            this.waypoints = waypoints;
            
            this.Game.Components.Add(this);
        }

        public int Health
        {
            get
            {
                return this.health;
            }
            set
            {
                this.health = value;
            }
        }

        public Queue<TimeSpan> WeaponTriggers
        {
            get
            {

                return this.weaponTriggers;
            }
        }

        public float Rotation
        {
            get
            {
                return this.rotation;
            }
        }

        public IEnumerable<Vector2> Waypoints
        {
            get
            {
                return this.waypoints;
            }
            set
            {
                this.waypoints = value;
            }
        }

        public TimeSpan SpawnTimestamp { get; private set; }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(this.position.X + (this.Width / 2.0f), this.position.Y + (this.Height / 2.0f));
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

        public double AngleToPlayer
        {
            get
            {
                return this.angleToPlayer;
            }
        }

        public void UpdateAngleToPlayer(double angle)
        {
            this.angleToPlayer = angle;
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
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
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            this.spriteBatch.Draw(
                this.sprite,
                new Vector2(this.position.X + this.sprite.Width / 2.0f, this.position.Y + sprite.Height / 2.0f),
                null,
                Color.Green,
                this.rotation,
                new Vector2(this.sprite.Width / 2.0f, this.sprite.Height / 2.0f),
                1.0f, 
                SpriteEffects.None, 
                0.0f);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {      
            this.enemyCurveX.PostLoop = CurveLoopType.Cycle;
            this.enemyCurveY.PostLoop = CurveLoopType.Cycle;

            this.enemyCurveX.PreLoop = CurveLoopType.Cycle;
            this.enemyCurveY.PreLoop = CurveLoopType.Cycle;

            this.enemyCurveX.ComputeTangents(CurveTangent.Smooth);
            this.enemyCurveY.ComputeTangents(CurveTangent.Smooth);

            var previousPositionX = this.position.X;
            var previousPositionY = this.position.Y;

            this.position.X = this.enemyCurveX.Evaluate((float)gameTime.TotalGameTime.TotalSeconds);
            this.position.Y = this.enemyCurveY.Evaluate((float)gameTime.TotalGameTime.TotalSeconds);

            this.rotation = (float)Math.Atan2(this.position.Y - previousPositionY, this.position.X - previousPositionX) + MathHelper.PiOver2;

            base.Update(gameTime);
        }
    }
}
