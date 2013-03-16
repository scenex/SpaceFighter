﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WeaponPlayerA : Weapon
    {
        private ICameraService cameraService;
    
        private double elapsedShotInterval;
        
        public WeaponPlayerA(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.Rotation = -MathHelper.PiOver2;
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.SpriteManager = new SpriteManager(PlayerState.Respawn, 23, 48);
            
            this.SpriteManager.AddStillSprite(
                PlayerState.Alive,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Alive"));

            this.SpriteManager.AddStillSprite(
                PlayerState.Dying,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Dead"));

            this.SpriteManager.AddStillSprite(
                PlayerState.Dead,
                this.Game.Content.Load<Texture2D>("Sprites/Turrets/Dead"));

            this.SpriteManager.AddStillSprite(
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
            switch (this.UpgradeLevel)
            {
                // Todo: Move to strategy WeaponA1
                case 0:
                    if (this.elapsedShotInterval > 0.1)
                    {
                        const int Offset = 105 / 2 - 30;

                        this.Shots.Add(
                            new ShotA(
                                 new Vector2(
                                    this.Position.X - (this.SpriteShot.Width / 2.0f) + Offset * ((float)Math.Cos(this.Rotation)),   // Center shot and then add r*cos(angle)
                                    this.Position.Y - (this.SpriteShot.Height / 2.0f) + Offset * ((float)Math.Sin(this.Rotation))),  // Center shot and then add r*sin(angle)                   
                                this.SpriteShot.Width,
                                this.SpriteShot.Height,
                                this.SpriteShotDataCached,
                                20,
                                this.Rotation));

                        this.elapsedShotInterval = 0;
                        this.TriggerWeaponFiredEvent(this, null);
                    }
                break;

                // Todo: Move to strategy WeaponA2
                case 1:
                    if (this.elapsedShotInterval > 0.1)
                    {
                        const int Offset = 105 / 2 - 30;

                        this.Shots.Add(
                            new ShotA(
                                 new Vector2(
                                    this.Position.X - (this.SpriteShot.Width / 2.0f) + Offset * ((float)Math.Cos(this.Rotation)) - 8,   // Center shot and then add r*cos(angle)
                                    this.Position.Y - (this.SpriteShot.Height / 2.0f) + Offset * ((float)Math.Sin(this.Rotation))),  // Center shot and then add r*sin(angle)                   
                                this.SpriteShot.Width,
                                this.SpriteShot.Height,
                                this.SpriteShotDataCached,
                                20,
                                this.Rotation));

                         this.Shots.Add(
                            new ShotA(
                                 new Vector2(
                                    this.Position.X - (this.SpriteShot.Width / 2.0f) + Offset * ((float)Math.Cos(this.Rotation)) + 8,   // Center shot and then add r*cos(angle)
                                    this.Position.Y - (this.SpriteShot.Height / 2.0f) + Offset * ((float)Math.Sin(this.Rotation))),  // Center shot and then add r*sin(angle)                   
                                this.SpriteShot.Width,
                                this.SpriteShot.Height,
                                this.SpriteShotDataCached,
                                20,
                                this.Rotation));

                        this.elapsedShotInterval = 0;
                        this.TriggerWeaponFiredEvent(this, null);
                    }
                break;
                    
            }


        }

        protected override void UpdateGameTime(GameTime gameTime)
        {
            this.elapsedShotInterval += gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void UpdateShots()
        {
            foreach (var shot in this.Shots)
            {
                shot.Position = 
                    new Vector2(
                        (shot.Position.X + (float)Math.Cos(shot.Angle) * 10),
                        (shot.Position.Y + (float)Math.Sin(shot.Angle) * 10));
            }
        }

        protected override void DrawShots()
        {
            this.SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

            foreach (var shot in this.Shots)
            {
                this.SpriteBatch.Draw(this.SpriteShot, shot.Position, Color.White);
            }

            this.SpriteBatch.End();
        }

        protected override void DrawTurret()
        {
            this.SpriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                this.SpriteManager.GetCurrentShader(),
                cameraService.GetTransformation());

            this.SpriteBatch.Draw(
                this.SpriteManager.GetCurrentSprite(), 
                this.Position, 
                null, 
                Color.White, 
                this.Rotation,
                new Vector2(this.SpriteManager.GetCurrentSprite().Width / 2.0f, this.SpriteManager.GetCurrentSprite().Height / 2.0f), 
                1.0f, 
                SpriteEffects.None, 
                0.0f);
            

            this.SpriteBatch.End();
        }
    }
}
