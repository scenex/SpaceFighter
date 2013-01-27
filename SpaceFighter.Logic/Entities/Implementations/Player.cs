// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using StateMachine;

    /// <summary>
    /// The spaceship class which represent the player's spaceship.
    /// </summary>
    public class Player : DrawableGameComponent, IPlayer
    {   
        private readonly Game game;
        private SpriteBatch spriteBatch;

        private ICameraService cameraService;
        
        private StateMachine<Action<double>> stateMachine;

        private double deadToRespawnTimer;

        private int healthReplenishCounter;

        private SpriteManager spriteManager;

        private int health;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            this.Health = 100;
            this.Rotation = -MathHelper.PiOver2;

            this.game = game;
            this.Position = startPosition;
        }

        public event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        public event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;
        public event EventHandler<HealthChangedEventArgs> HealthChanged; 

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Color[] ColorData { get; private set; }
        
        public int Health
        {
            get
            {
                return this.health;
            }
            private set
            {
                this.health = value;

                if (this.HealthChanged != null)
                {
                    this.HealthChanged(this, new HealthChangedEventArgs(value));
                }
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

        public void Thrust(int amount)
        {
            this.Position =
                Vector2.Add(
                    new Vector2(
                        (float)Math.Cos(this.Rotation) * amount,
                        (float)Math.Sin(this.Rotation) * amount),
                    this.Position);
        }

        public void SubtractHealth(int amount)
        {
            this.Health -= amount;
        }

        public void AddHealth(int amount)
        {
            this.Health += amount;
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            this.InitializeStateMachine();
            base.Initialize();
        }

        private void InitializeStateMachine()
        {
            var dying = new State<Action<double>>(
                PlayerState.Dying,
                null,
                delegate
                    {
                        if (this.TransitionToStateDying != null)
                        {
                            this.TransitionToStateDying(this, new StateChangedEventArgs(PlayerState.Alive, PlayerState.Dying));
                        }
                    }, 
                null);

            var dead = new State<Action<double>>(
                PlayerState.Dead,
                elapsedTime => this.deadToRespawnTimer += elapsedTime,
                delegate
                    {       
                        if (this.TransitionToStateDead != null)
                        {
                            this.TransitionToStateDead(this, new StateChangedEventArgs(PlayerState.Dying, PlayerState.Dead));
                        }
                    }, 
                () => this.deadToRespawnTimer = 0);

            var respawn = new State<Action<double>>(
                PlayerState.Respawn,
                elapsedTime =>
                    {
                        this.healthReplenishCounter++; 

                        if(this.healthReplenishCounter % 2 == 0)
                        {
                            this.Health++;
                        }
                    },
                delegate
                    {
                        if (this.TransitionToStateRespawn != null)
                        {
                            this.TransitionToStateRespawn(this, new StateChangedEventArgs(PlayerState.Dead, PlayerState.Respawn));
                        }                       
                    },
                () => { this.healthReplenishCounter = 0; });

            var alive = new State<Action<double>>(
                PlayerState.Alive,
                null, 
                delegate
                    {
                        if (this.TransitionToStateAlive != null)
                        {
                            this.TransitionToStateAlive(this, new StateChangedEventArgs(PlayerState.Respawn, PlayerState.Alive));
                        }
                    },
                null);

            alive.AddTransition(dying, () => this.Health <= 0);
            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));
            dead.AddTransition(respawn, () => this.deadToRespawnTimer > 1000);
            respawn.AddTransition(alive, () => this.Health == 100);

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.spriteManager = new SpriteManager(PlayerState.Alive, 64, 64);

            this.spriteManager.AddStillSprite(
                PlayerState.Alive, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"));

            this.spriteManager.AddAnimatedSprite(
                PlayerState.Dying, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dying"));

            this.spriteManager.AddStillSprite(
                PlayerState.Dead, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dead"));

            this.spriteManager.AddStillSprite(
                PlayerState.Respawn, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"), 
                this.game.Content.Load<Effect>("Shaders/Transparency"),
                time => (float)(0.5f * Math.Sin(time * 20) + 0.5),
                "param1");

            this.UpdateSpriteColorData();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {       
            this.cameraService.Position = this.Position;

            this.stateMachine.Update();
            this.spriteManager.Update(this.stateMachine.CurrentState.Name, gameTime);

            if (this.stateMachine.CurrentState.Tag != null)
            {
                this.stateMachine.CurrentState.Tag(gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                this.spriteManager.GetCurrentShader(),
                cameraService.GetTransformation());

            this.spriteBatch.Draw(
                this.spriteManager.GetCurrentSprite(),
                this.Position,
                this.spriteManager.GetCurrentRectangle(),
                Color.White,
                this.Rotation,
                new Vector2((float)this.Width / 2, (float)this.Height / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateSpriteColorData()
        {
            this.ColorData = new Color[this.spriteManager.GetCurrentSprite().Width * this.spriteManager.GetCurrentSprite().Height];
            this.spriteManager.GetCurrentSprite().GetData(this.ColorData);
        }
    }
}
