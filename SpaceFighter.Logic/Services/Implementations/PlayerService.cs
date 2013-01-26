﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerService : GameComponent, IPlayerService
    {
        private Player player;
        private IPlayerWeaponService playerWeaponService;

        public PlayerService(Game game) : base(game)
        {
        }

        public event EventHandler<StateChangedEventArgs> TransitionToStateAlive;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDying;
        public event EventHandler<StateChangedEventArgs> TransitionToStateDead;
        public event EventHandler<StateChangedEventArgs> TransitionToStateRespawn;
        public event EventHandler<HealthChangedEventArgs> HealthChanged;

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

            this.player = new Player(this.Game, new Vector2((this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 16, (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) - 0)); // Todo: Eliminate magic number
            this.Game.Components.Add(this.player);

            this.player.TransitionToStateDying += this.OnTransitionToStateDying;
            this.player.TransitionToStateDead += this.OnTransitionToStateDead;
            this.player.TransitionToStateRespawn += this.OnTransitionToStateRespawn;
            this.player.TransitionToStateAlive += this.OnTransitionToStateAlive;
            this.player.HealthChanged += this.OnHealthChanged;

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
            this.player.SubtractHealth(shot.FirePower);
        }

        public void ReportPlayerHit(int damage)
        {
            this.player.SubtractHealth(damage);
        }

        public void RemoveShot(IShot shot)
        {
            this.playerWeaponService.Weapon.Shots.Remove(shot);
        }

        private void OnTransitionToStateDying(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.TransitionToStateDying != null)
            {
                this.TransitionToStateDying(this, stateChangedEventArgs);
            }
        }

        private void OnTransitionToStateDead(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.TransitionToStateDead != null)
            {
                this.TransitionToStateDead(this, stateChangedEventArgs);
            }
        }

        private void OnTransitionToStateRespawn(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.TransitionToStateRespawn != null)
            {
                this.TransitionToStateRespawn(this, stateChangedEventArgs);
            }
        }

        private void OnTransitionToStateAlive(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            if (this.TransitionToStateAlive != null)
            {
                this.TransitionToStateAlive(this, stateChangedEventArgs);
            }
        }

        private void OnHealthChanged(object sender, HealthChangedEventArgs healthChangedEventArgs)
        {
            if (this.HealthChanged != null)
            {
                this.HealthChanged(this, healthChangedEventArgs);
            }
        }
    }
}
