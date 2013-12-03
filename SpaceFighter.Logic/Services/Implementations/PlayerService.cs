// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Players;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerService : GameComponent, IPlayerService
    {
        private PlayerA player;

        private readonly IAudioService audioService;
        private readonly IPlayerFactory playerFactory;
        private readonly ITerrainService terrainService;

        public PlayerService(Game game, IAudioService audioService, IPlayerFactory playerFactory, ITerrainService terrainService) : base(game)
        {
            this.audioService = audioService;
            this.playerFactory = playerFactory;
            this.terrainService = terrainService;
        }

        public event EventHandler<StateChangedEventArgs> ShipRespawning;
        public event EventHandler<StateChangedEventArgs> ShipReady;
        public event EventHandler<StateChangedEventArgs> ShipExploding;

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
                return this.player.Weapon.Shots;
            }
        }

        public void SpawnPlayer()
        {
            this.player = this.playerFactory.Create(
                new Vector2(
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 16) * 6,
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight / 9) * 8)); // Todo: Eliminate magic numbers)

            this.player.ShipExploding += this.OnShipExploding;
            this.player.ShipRespawning += this.OnShipRespawning;
            this.player.ShipReady += this.OnShipReady;
            this.player.Weapon.WeaponFired += this.OnWeaponFired;

            this.Game.Components.Add(this.player);
        }

        public void UnspawnPlayer()
        {
            this.player.Dispose();
        }

        public void Fire()
        {
            this.player.Weapon.FireWeapon();
        }

        // Todo: Why shifting by half player's height?
        public void MoveUp()
        {
            if (this.player.Position.Y - this.player.Height / 2.0 >= 0)
            {
                this.player.Move(new Vector2(0,-3));
            }
        }

        // Todo: Why shifting by half player's height?
        public void MoveDown()
        {
            if (this.player.Position.Y + this.player.Height / 2.0 <= this.terrainService.LevelHeight)
            {
                this.player.Move(new Vector2(0, 3));
            }
        }

        // Todo: Why shifting by half player's width?
        public void MoveLeft()
        {
            if (this.player.Position.X - this.player.Width / 2.0 >= 0)
            {
                this.player.Move(new Vector2(-3, 0));
            }
        }

        // Todo: Why shifting by half player's width?
        public void MoveRight()
        {
            if (this.player.Position.X + this.player.Width / 2.0 <= this.terrainService.LevelWidth)
            {
                this.player.Move(new Vector2(3, 0));
            }
        }

        public void ReportPlayerHit(IShot shot)
        {
            this.player.SubtractHealth(shot.FirePower);
        }

        public void ReportPlayerHit(int damage)
        {
            this.player.SubtractHealth(damage);
        }

        public void RemoveShot(IShot shot)
        {
            this.player.Weapon.Shots.Remove(shot);
        }

        private void OnShipExploding(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.audioService.PlaySound("explosion");

            if (this.ShipExploding != null)
            {
                this.ShipExploding(this, stateChangedEventArgs);
            }
        }

        private void OnShipRespawning(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.ShipRespawning != null)
            {
                this.ShipRespawning(this, stateChangedEventArgs);
            }
        }

        private void OnShipReady(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.ShipReady != null)
            {
                this.ShipReady(this, stateChangedEventArgs);
            }
        }

        private void OnWeaponFired(object sender, EventArgs e)
        {
            this.audioService.PlaySound("shot");
        }
    }
}
