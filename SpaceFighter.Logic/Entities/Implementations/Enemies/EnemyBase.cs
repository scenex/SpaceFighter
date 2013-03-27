﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public abstract class EnemyBase : DrawableGameComponent, IEnemy
    {
        private SpriteBatch spriteBatch;

        private ICameraService cameraService;
        protected Vector2 distanceToPlayer;      

        protected StateMachine<Action<double>> stateMachine;
        protected SpriteManager spriteManager;

        protected EnemyBase(Game game, Vector2 startPosition) : base(game)
        {
            this.Position = startPosition;           
            this.Game.Components.Add(this);
        }

        protected abstract void InitializeStateMachine();
        protected abstract void LoadSprites();
        protected abstract void UpdatePosition();
        protected abstract void UpdateWeapon(TimeSpan elapsed);
        
        public abstract IWeapon Weapon { get; }

        public int Health { get; protected set; }
        public float Rotation { get; private set; }
        public Vector2 Position { get; protected set; }
        public float AngleToPlayer { get; private set; }
        public Color[] ColorData { get; private set; }

        public int Width
        {
            get
            {
                return this.spriteManager.GetCurrentRectangle().Width;
            }
        }

        public int Height
        {
            get
            {
                return this.spriteManager.GetCurrentRectangle().Height;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(this.Position.X + (this.Width / 2.0f), this.Position.Y + (this.Height / 2.0f));
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void SubtractHealth(int amount)
        {
            this.Health -= amount;
        }

        public void AddHealth(int amount)
        {
            this.Health += amount;
        }

        public void UpdateAngleToPlayer(float angle)
        {
            this.AngleToPlayer = angle;
        }

        public void UpdateDistanceToPlayer(Vector2 distance)
        {
            this.distanceToPlayer = distance;
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            this.InitializeStateMachine();
            base.Initialize();
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.LoadSprites();
            this.UpdateSpriteColorData();

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
                this.spriteManager.GetCurrentSprite(),
                this.Position,
                this.spriteManager.GetCurrentRectangle(),
                Color.White,
                this.Rotation,
                new Vector2(this.Width / 2.0f, this.Height / 2.0f),
                1.0f, 
                SpriteEffects.None, 
                0.0f);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.stateMachine.Update();
            this.spriteManager.Update(this.stateMachine.CurrentState.Name, gameTime);
           
            var previousPositionX = this.Position.X;
            var previousPositionY = this.Position.Y;
            
            this.UpdatePosition();
            this.UpdateWeapon(gameTime.ElapsedGameTime);
            this.Rotation = (float)Math.Atan2(this.Position.Y - previousPositionY, this.Position.X - previousPositionX); // Some kind of smoothing here?

            base.Update(gameTime);
        }
       
        private void UpdateSpriteColorData()
        {
            this.ColorData = new Color[this.spriteManager.GetCurrentSprite().Width * this.spriteManager.GetCurrentSprite().Height];
            this.spriteManager.GetCurrentSprite().GetData(this.ColorData);
        }
    }
}
