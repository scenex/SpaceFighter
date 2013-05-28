// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public abstract class EnemyBase : DrawableGameComponent, IEnemy
    {
        private SpriteBatch spriteBatch;

        private readonly ICameraService cameraService;
        private readonly IPathFindingService pathFindingService;

        protected StateMachine<Action<double>> stateMachine;
        protected SpriteManager spriteManager;

        protected EnemyBase(Game game, IPathFindingService pathFindingService, Vector2 startPosition) : base(game)
        {
            this.Position = startPosition;           
            this.Game.Components.Add(this);

            this.pathFindingService = pathFindingService;
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
        }

        protected abstract void InitializeStateMachine();
        protected abstract void LoadSprites();
        protected abstract void UpdatePosition();
        protected abstract void UpdateWeapon(TimeSpan elapsed);
        
        public abstract IWeapon Weapon { get; }
        public abstract Queue<Vector2> Waypoints { get; }

        public Vector2 PlayerPosition { get; set; }
        public Vector2 Position { get; protected set; }

        public int Health { get; protected set; }
        public float Rotation { get; private set; }
        public Color[] ColorData { get; private set; }

        public bool IsHealthSubtracted { get; protected set; }
        public bool IsHealthAdded { get; protected set; }

        protected IPathFindingService PathFindingService
        {
            get
            {
                return this.pathFindingService;
            }
        }

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
            this.IsHealthSubtracted = true;
        }

        public void AddHealth(int amount)
        {
            this.Health += amount;
            this.IsHealthAdded = true;
        }

        public override void Initialize()
        {
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

            if (this.stateMachine.CurrentState.Tag != null)
            {
                this.stateMachine.CurrentState.Tag(gameTime.ElapsedGameTime.TotalMilliseconds);
            }

            this.spriteManager.Update(this.stateMachine.CurrentState.Name, gameTime);
           
            var previousPositionX = this.Position.X;
            var previousPositionY = this.Position.Y;
            
            this.UpdatePosition();
            this.UpdateWeapon(gameTime.ElapsedGameTime);
            this.Rotation = (float)Math.Atan2(this.Position.Y - previousPositionY, this.Position.X - previousPositionX); // Todo: Add some kind of smoothing here, so steering behavior like wandering are not too jittery..

            base.Update(gameTime);
        }
       
        private void UpdateSpriteColorData()
        {
            this.ColorData = new Color[this.spriteManager.GetCurrentSprite().Width * this.spriteManager.GetCurrentSprite().Height];
            this.spriteManager.GetCurrentSprite().GetData(this.ColorData);
        }
    }
}
