// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerWeaponA : Weapon
    {
        private ICameraService cameraService;

        private readonly IList<IShot> shots;        
        private double elapsedShotInterval;
        
        public PlayerWeaponA(Game game) : base(game)
        {
            this.shots = new List<IShot>();
        }

        public override void Initialize()
        {
            this.Rotation = -MathHelper.PiOver2;
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteManager = new SpriteManager(PlayerState.Respawn, 23, 48);
            
            this.spriteManager.AddStillSprite(
                PlayerState.Alive,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Alive"));

            this.spriteManager.AddStillSprite(
                PlayerState.Dying,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Dead"));

            this.spriteManager.AddStillSprite(
                PlayerState.Dead,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Dead"));

            this.spriteManager.AddStillSprite(
                PlayerState.Respawn,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Alive"),
                this.Game.Content.Load<Effect>("Shaders/Transparency"),
                time => (float)(0.5f * Math.Sin(time * 20) + 0.5),
                "param1");

            this.LoadShot("Sprites/Shot");
            base.LoadContent();
        }

        public override void FireWeapon()
        {
            if (this.elapsedShotInterval > 0.1)
            {
                const int Offset = 105 / 2 - 30;

                this.shots.Add(
                    new ShotA(
                         new Vector2(
                            this.Position.X - (this.spriteShot.Width / 2.0f) + Offset * ((float)Math.Cos(this.Rotation)),   // Center shot and then add r*cos(angle)
                            this.Position.Y - (this.spriteShot.Height / 2.0f) + Offset * ((float)Math.Sin(this.Rotation))),  // Center shot and then add r*sin(angle)                   
                        this.spriteShot.Width,
                        this.spriteShot.Height,
                        this.spriteShotDataCached,
                        20,
                        this.Rotation));

                this.elapsedShotInterval = 0;
                this.TriggerWeaponFiredEvent(this, null);
            }
        }

        protected override void UpdateGameTime(GameTime gameTime)
        {
            this.elapsedShotInterval += gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void UpdateShots()
        {
            foreach (var shot in this.shots)
            {
                shot.Position = 
                    new Vector2(
                        (shot.Position.X + (float)Math.Cos(shot.Angle) * 10),
                        (shot.Position.Y + (float)Math.Sin(shot.Angle) * 10));
            }
        }

        protected override void UpdateTurret()
        {
            // Not used yet..
        }

        protected override void DrawShots()
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            foreach (var shot in this.shots)
            {
                this.spriteBatch.Draw(this.spriteShot, shot.Position, Color.White);
            }

            this.spriteBatch.End();
        }

        protected override void DrawTurret()
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
                null, 
                Color.White, 
                this.Rotation,
                new Vector2(this.spriteManager.GetCurrentSprite().Width / 2.0f, this.spriteManager.GetCurrentSprite().Height / 2.0f), 
                1.0f, 
                SpriteEffects.None, 
                0.0f);
            

            this.spriteBatch.End();
        }

        public override IList<IShot> Shots
        {
            get
            {
                return this.shots;
            }
        }
    }
}
