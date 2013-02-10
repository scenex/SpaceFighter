// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.AI;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyBase : DrawableGameComponent, IEnemy
    {
        private Vector2 position;
        private SpriteBatch spriteBatch;
        private Color[] colorData;
        private int health = 100;       
        private float rotation;
        private double angleToPlayer;

        private ICameraService cameraService;
        private StateMachine<Action<double>> stateMachine;
        private SpriteManager spriteManager;
        private bool isAlive;
        private Vector2 distanceToPlayer;
        private ISteering steeringStrategy;

        public EnemyBase(Game game, Vector2 startPosition) : base(game)
        {
            this.isAlive = true;
            this.position = startPosition;
            this.steeringStrategy = new SteeringSeek();
            
            this.Game.Components.Add(this);
        }

        public int Health
        {
            get
            {
                return this.health;
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAlive
        {
            get
            {
                return this.isAlive;
            }
        }

        public void SubtractHealth(int amount)
        {
            this.health -= amount;
        }

        public void AddHealth(int amount)
        {
            this.health += amount;
        }

        public float Rotation
        {
            get
            {
                return this.rotation;
            }
        }

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

        private void InitializeStateMachine()
        {
            var alive = new State<Action<double>>(
                EnemyState.Alive,
                null,
                null,
                null);

            var dying = new State<Action<double>>(
                EnemyState.Dying,
                null,
                null,
                null);

            var dead = new State<Action<double>>(
                EnemyState.Dead,
                null,
                () => this.isAlive = false,
                null);

            alive.AddTransition(dying, () => this.Health <= 0);
            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.spriteManager = new SpriteManager(EnemyState.Alive, 108, 128);

            this.spriteManager.AddStillSprite(
                EnemyState.Alive,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Alive"));

            this.spriteManager.AddAnimatedSprite(
                EnemyState.Dying,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Dying"));

            this.spriteManager.AddStillSprite(
                EnemyState.Dead,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Dead"));

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
                this.position,
                this.spriteManager.GetCurrentRectangle(),
                Color.White,
                this.rotation,
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
           
            var previousPositionX = this.position.X;
            var previousPositionY = this.position.Y;

            this.position = this.steeringStrategy.AdvancePosition(this.position, this.distanceToPlayer, this.rotation);
            this.rotation = (float)Math.Atan2(this.position.Y - previousPositionY, this.position.X - previousPositionX);

            base.Update(gameTime);
        }

        private void UpdateSpriteColorData()
        {
            this.colorData = new Color[this.spriteManager.GetCurrentSprite().Width * this.spriteManager.GetCurrentSprite().Height];
            this.spriteManager.GetCurrentSprite().GetData(this.ColorData);
        }
    }
}
