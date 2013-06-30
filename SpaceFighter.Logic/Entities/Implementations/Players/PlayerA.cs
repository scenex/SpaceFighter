// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Players
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    /// <summary>
    /// The spaceship class which represent the player's spaceship.
    /// </summary>
    public class PlayerA : DrawableGameComponent, IPlayer
    {   
        private SpriteBatch spriteBatch;
        private ICameraService cameraService;       
        private StateMachine<Action<double>> stateMachine;
        private double deadToRespawnTimer;
        private int healthReplenishCounter;
        private SpriteManager spriteManager;
        private int health;
        private Weapon weapon;
        private float thrustTotal;

        private const float ThrustIncrement = 0.2f;
        private const float ThrustFriction = 0.05f;

        public PlayerA(Game game, Vector2 startPosition) : base(game)
        {
            this.weapon = new WeaponPlayerA(this.Game) { DrawOrder = 1 };
            this.Game.Components.Add(this.weapon);

            this.Health = 100;
            this.Rotation = -MathHelper.PiOver2;
            this.Position = startPosition;
        }

        public event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        public event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;
        public event EventHandler<HealthChangedEventArgs> HealthChanged;

        public Vector2 Position { get; private set; }
        public float Rotation { get; private set; }
        public Color[] ColorData { get; private set; }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(
                    (int)this.Position.X - this.spriteManager.GetCurrentRectangle().Width / 2,
                    (int)this.Position.Y - this.spriteManager.GetCurrentRectangle().Height / 2,
                    this.spriteManager.GetCurrentRectangle().Width,
                    this.spriteManager.GetCurrentRectangle().Height);
            }
        }

        public IWeapon Weapon
        {
            get
            {
                return this.weapon;
            }
        }

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

        public void Thrust()
        {
            this.thrustTotal += ThrustIncrement;
        }

        public void SetRotation(float angle)
        {
            this.Rotation += angle;
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
                        this.thrustTotal = 0;

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
            this.spriteManager = new SpriteManager(PlayerState.Alive, 80, 80);

            this.spriteManager.AddStillSprite(
                PlayerState.Alive, 
                this.Game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"));

            this.spriteManager.AddAnimatedSprite(
                PlayerState.Dying, 
                this.Game.Content.Load<Texture2D>("Sprites/Spaceship/Dying"));

            this.spriteManager.AddStillSprite(
                PlayerState.Dead, 
                this.Game.Content.Load<Texture2D>("Sprites/Spaceship/Dead"));

            this.spriteManager.AddStillSprite(
                PlayerState.Respawn, 
                this.Game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"), 
                this.Game.Content.Load<Effect>("Shaders/Transparency"),
                time => (float)(0.5f * Math.Sin(time * 20) + 0.5),
                "param1");

            this.UpdateSpriteColorData();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = Vector2.Add(new Vector2((float)Math.Cos(this.Rotation) * this.thrustTotal, (float)Math.Sin(this.Rotation) * this.thrustTotal), this.Position);
            this.thrustTotal = MathHelper.Clamp(this.thrustTotal -= ThrustFriction, 0.0f, 3.0f);

            this.weapon.Position = this.Position;
            this.cameraService.Position = this.Position;
          
            this.stateMachine.Update();
            this.spriteManager.Update(this.stateMachine.CurrentState.Name, gameTime);
            this.weapon.SpriteManager.Update(this.stateMachine.CurrentState.Name, gameTime);

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
                this.cameraService.GetTransformation());

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

        private void UpdateSpriteColorData()
        {
            this.ColorData = new Color[this.spriteManager.GetCurrentSprite().Width * this.spriteManager.GetCurrentSprite().Height];
            this.spriteManager.GetCurrentSprite().GetData(this.ColorData);
        }
    }
}
