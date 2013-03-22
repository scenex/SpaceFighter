// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Implementations.WeaponStrategies;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WeaponPlayerA : Weapon
    {
        private ICameraService cameraService;

        private IWeaponStrategy weaponStrategy;
        private readonly WeaponStrategyA1 weaponStrategyA1;
        private readonly WeaponStrategyA2 weaponStrategyA2;

        private double elapsedShotInterval;
        
        public WeaponPlayerA(Game game) : base(game)
        {
            this.weaponStrategy = new WeaponStrategyA1();
            this.weaponStrategyA1 = new WeaponStrategyA1();
            this.weaponStrategyA2 = new WeaponStrategyA2();
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
                case 1:
                    this.weaponStrategy = this.weaponStrategyA1;

                    if (this.weaponStrategy.Execute(
                        () => this.TriggerWeaponFiredEvent(this, null),
                        this.elapsedShotInterval,
                        this.Shots,
                        this.Position,
                        this.Rotation,
                        this.SpriteShot.Width,
                        this.SpriteShot.Height,
                        this.SpriteShotDataCached))
                    {
                        this.elapsedShotInterval = 0;
                    }
                    
                break;

                case 2:
                    this.weaponStrategy = this.weaponStrategyA2;

                    if (this.weaponStrategy.Execute(
                        () => this.TriggerWeaponFiredEvent(this, null),
                        this.elapsedShotInterval,
                        this.Shots,
                        this.Position,
                        this.Rotation,
                        this.SpriteShot.Width,
                        this.SpriteShot.Height,
                        this.SpriteShotDataCached))
                    {
                        this.elapsedShotInterval = 0;
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
