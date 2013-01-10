﻿// -----------------------------------------------------------------------
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

        private Rectangle spriteRectangle;

        private Texture2D spriteAlive;
        private Texture2D spriteDead;
        private Texture2D spriteDying;

        private ICameraService cameraService;

        private float totalElapsed;
        private int currentFrame;

        private const int FrameCount = 16;
        private const float TimePerFrame = 0.0166667f * 3;

        private StateMachine<Action<float>> stateMachine;

        private bool isRespawnAllowed;

        public Player(Game game, Vector2 startPosition) : base(game)
        {
            Health = 100;
            this.game = game;
            this.Position = startPosition;
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Color[] ColorData { get; private set; }
        public int Health { get; set; }

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

        private Texture2D GetCurrentSprite()
        {
            switch(this.stateMachine.CurrentState.Name)
            {
                case PlayerState.Alive:
                    return spriteAlive;

                case PlayerState.Dying:
                    return spriteDying;

                case PlayerState.Dead:
                    return spriteDead;

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
            var alive = new State<Action<float>>("Alive", null, null, null);
            var dying = new State<Action<float>>("Dying", null, null, null);
            //var dead = new State<Action<float>>("dead", null, EnterLead, null);
            //var respawn = new State<Action<float>>("respawn", Rally, EnterRally, null);

            alive.AddTransition(dying, () => this.Health <= 0);
            //dying.AddTransition(dead, () => Leader != null);
            //dead.AddTransition(respawn, () => Leader == null);
            //respawn.AddTransition(alive, () => Members.Count < FormationSize);

            this.stateMachine = new StateMachine<Action<float>>(alive);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.spriteAlive = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Alive");
            this.spriteDying = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dying");
            this.spriteDead = this.game.Content.Load<Texture2D>("Sprites/Spaceship/Dead");
            this.spriteRectangle = new Rectangle(0, 0, this.spriteAlive.Width, this.spriteAlive.Height);

            this.UpdateSpriteColorData();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.cameraService.Position = this.Position;
            this.stateMachine.Update();
            //this.stateMachine.CurrentState.Tag(gameTime.ElapsedGameTime);

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
                null,
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
