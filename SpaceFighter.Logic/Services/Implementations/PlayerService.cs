// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Implementations;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerService : GameComponent, IPlayerService
    {
        private Player player;
        private IPlayerWeaponService playerWeaponService;
        private int screenWidth;
        private int screenHeight;

        public PlayerService(Game game) : base(game)
        {
        }

        public IPlayer Player
        {
            get
            {
                return this.player;
            }
        }

        public IEnumerable<IShot> Shots
        {
            get
            {
                return this.playerWeaponService.Weapon.Shots;
            }
        }

        public override void Initialize()
        {
            this.playerWeaponService = (IPlayerWeaponService)this.Game.Services.GetService((typeof(IPlayerWeaponService)));
            this.screenWidth = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferWidth;
            this.screenHeight = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferHeight;

            this.player = new Player(this.Game, new Vector2((this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 16, (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 0));
            this.Game.Components.Add(this.player);

            base.Initialize();
        }

        public void RotateLeft()
        {
            this.player.Rotation += 0.05f;
        }

        public void RotateRight()
        {
            this.player.Rotation -= 0.05f;
        }

        public void Fire()
        {
            this.playerWeaponService.FireWeapon(new Vector2(this.player.Position.X, this.player.Position.Y), player.Height / 2, this.player.Rotation);
        }

        public void Thrust()
        {
            this.player.Thrust(3);
        }

        public void ReportPlayerHit(IShot shot)
        {
            // Subtract firepower from player's health
        }

        public void RemoveShot(IShot shot)
        {
            this.playerWeaponService.Weapon.Shots.Remove(shot);
        }
    }
}
