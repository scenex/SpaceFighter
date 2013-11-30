﻿// -----------------------------------------------------------------------
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

        public PlayerService(Game game, IAudioService audioService, IPlayerFactory playerFactory) : base(game)
        {
            this.audioService = audioService;
            this.playerFactory = playerFactory;
        }

        public event EventHandler<StateChangedEventArgs> ShipInvincible;
        public event EventHandler<StateChangedEventArgs> ShipVulnerable;
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

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SpawnPlayer()
        {
            this.player = this.playerFactory.Create(
                new Vector2(
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 16) * 6,
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight / 9) * 8)); // Todo: Eliminate magic numbers)

            this.player.ShipExploding += this.OnShipExploding;
            this.player.ShipInvincible += this.OnShipInvincible;
            this.player.ShipVulnerable += this.OnShipVulnerable;
            this.player.Weapon.WeaponFired += this.OnWeaponFired;

            this.Game.Components.Add(this.player);
        }

        public void UnspawnPlayer()
        {
            this.player.Dispose();
        }

        public void RotateLeft()
        {
            this.player.SetRotation(+0.05f);
        }

        public void RotateRight()
        {
            this.player.SetRotation(-0.05f);
        }

        public void Fire()
        {
            this.player.Weapon.FireWeapon();
        }

        public void Thrust()
        {
            this.player.Thrust();
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

        private void OnShipInvincible(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.ShipInvincible != null)
            {
                this.ShipInvincible(this, stateChangedEventArgs);
            }
        }

        private void OnShipVulnerable(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.ShipVulnerable != null)
            {
                this.ShipVulnerable(this, stateChangedEventArgs);
            }
        }

        private void OnWeaponFired(object sender, EventArgs e)
        {
            this.audioService.PlaySound("shot");
        }
    }
}
