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
        private double respawnToAliveTimer ;

        private double totalElapsedTime;

        private SpriteManager spriteManager;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            Health = 100;
            this.game = game;
            this.Position = startPosition;
        }

        public event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        public event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Color[] ColorData { get; private set; }
        public int Health { get; private set; }

        public int Width
        {
            get
            {
                return this.spriteManager.GetRectangle().Width;
            }
        }

        public int Height
        {
            get
            {
                return this.spriteManager.GetRectangle().Height;
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
                        (float)Math.Cos(this.Rotation - MathHelper.PiOver2) * amount,
                        (float)Math.Sin(this.Rotation - MathHelper.PiOver2) * amount),
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
                elapsedTime => { this.respawnToAliveTimer += elapsedTime; this.totalElapsedTime += elapsedTime; },
                delegate
                    {
                        this.Health = 100;

                        if (this.TransitionToStateRespawn != null)
                        {
                            this.TransitionToStateRespawn(this, new StateChangedEventArgs(PlayerState.Dead, PlayerState.Respawn));
                        }                       
                    },
                () => { this.respawnToAliveTimer = 0; this.totalElapsedTime = 0; });

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
            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone);
            dead.AddTransition(respawn, () => this.deadToRespawnTimer > 1000);
            respawn.AddTransition(alive, () => this.respawnToAliveTimer > 4000);

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.spriteManager = new SpriteManager(PlayerState.Alive);

            this.spriteManager.AddSprite(
                PlayerState.Alive, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"), 
                false);

            this.spriteManager.AddSprite(
                PlayerState.Dying, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dying"), 
                true);

            this.spriteManager.AddSprite(
                PlayerState.Dead, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dead"), 
                false);

            this.spriteManager.AddSprite(
                PlayerState.Respawn, 
                this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive"), 
                this.game.Content.Load<Effect>("Shaders/Transparency"),
                false);

            this.spriteManager.SetRectangle(PlayerState.Alive);

            this.UpdateSpriteColorData();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {       
            if (this.stateMachine.CurrentState.Name == PlayerState.Respawn) // <- move logic to spritemanager, assign shader to state
            {
                var temp = 0.5f * Math.Sin(this.totalElapsedTime / 50) + 0.5f;
                this.spriteManager.GetCurrentShader().Parameters["param1"].SetValue((float)temp);
            }

            this.cameraService.Position = this.Position;

            this.stateMachine.Update();
            this.spriteManager.Update(this.stateMachine.CurrentState.Name);

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
                this.spriteManager.GetCurrentRectangle(gameTime),
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
