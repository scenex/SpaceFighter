// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations
{
    using System;
    using System.Collections.Generic;

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

        private Rectangle spriteRectangle;

        private readonly Dictionary<string, Texture2D> sprites;

        private ICameraService cameraService;

        private float totalElapsed;
        private int currentFrame;

        private const int FrameCount = 16;
        private const float TimePerFrame = 0.0166667f * 3;
        
        private StateMachine<Action<double>> stateMachine;

        private double deadToRespawnTimer;
        private double respawnToAliveTimer ;

        private Effect effect;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            Health = 100;
            this.game = game;
            this.Position = startPosition;
            this.sprites = new Dictionary<string, Texture2D>();
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
                return this.spriteRectangle.Width;
            }
        }

        public int Height
        {
            get
            {
                return this.spriteRectangle.Height;
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

        private Texture2D GetCurrentSprite()
        {
            switch(this.stateMachine.CurrentState.Name)
            {
                case PlayerState.Alive:
                    return this.sprites[PlayerState.Alive];

                case PlayerState.Dying:
                    return this.sprites[PlayerState.Dying];

                case PlayerState.Dead:
                    return this.sprites[PlayerState.Dead];

                case PlayerState.Respawn:
                    return this.sprites[PlayerState.Alive];

                default:
                    throw new NotImplementedException();
            }
        }

        private Rectangle GetCurrentRectangle(GameTime gameTime)
        {
            Rectangle currentRectangle;

            switch (this.stateMachine.CurrentState.Name)
            {
                case PlayerState.Dying:
                    currentRectangle = new Rectangle(0 + this.currentFrame * this.Width, 0, this.Width, this.Height);
                    this.totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (this.currentFrame != FrameCount - 1)
                    {
                        if (this.totalElapsed > TimePerFrame)
                        {
                            this.currentFrame++;
                            this.currentFrame = this.currentFrame % FrameCount;
                            this.totalElapsed = 0;
                        }
                    }
                    break;

                default:
                    currentRectangle = this.spriteRectangle;
                    this.currentFrame = 0;
                    break;
            }

            return currentRectangle;
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
                elapsedTime => this.respawnToAliveTimer += elapsedTime, 
                delegate
                    {
                        this.Health = 100;

                        if (this.TransitionToStateRespawn != null)
                        {
                            this.TransitionToStateRespawn(this, new StateChangedEventArgs(PlayerState.Dead, PlayerState.Respawn));
                        }                       
                    }, 
                () => this.respawnToAliveTimer = 0);

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
            dying.AddTransition(dead, () => this.currentFrame == FrameCount - 1);
            dead.AddTransition(respawn, () => this.deadToRespawnTimer > 1000);
            respawn.AddTransition(alive, () => this.respawnToAliveTimer > 4000);

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.sprites[PlayerState.Alive] = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive");
            this.sprites[PlayerState.Dying] = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dying");
            this.sprites[PlayerState.Dead] = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dead");
            this.effect = this.game.Content.Load<Effect>("Shaders/Transparency");

            this.spriteRectangle = new Rectangle(0, 0, this.sprites[PlayerState.Alive].Width, this.sprites[PlayerState.Alive].Height);

            this.UpdateSpriteColorData();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.cameraService.Position = this.Position;
            this.stateMachine.Update();

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
                effect,
                cameraService.GetTransformation());

            this.spriteBatch.Draw(
                this.GetCurrentSprite(),
                this.Position,
                this.GetCurrentRectangle(gameTime),
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
            // Obtain color information for subsequent per pixel collision detection
            this.ColorData = new Color[this.GetCurrentSprite().Width * this.GetCurrentSprite().Height];
            this.GetCurrentSprite().GetData(this.ColorData);
        }
    }

    public static class PlayerState
    {
        public const string Alive = "Alive";
        public const string Dying = "Dying";
        public const string Dead = "Dead";
        public const string Respawn = "Respawn";
    }
}
