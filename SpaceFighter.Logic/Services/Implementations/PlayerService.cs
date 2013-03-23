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
        private IAudioService audioService;

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
                return this.player.Weapon.Shots;
            }
        }

        public override void Initialize()
        {
            this.audioService = (IAudioService)this.Game.Services.GetService((typeof(IAudioService)));

            this.player = new PlayerA
                (this.Game, 
                new Vector2(
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) + 40, 
                    (this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight / 2) + 300)); // Todo: Eliminate magic numbers

            this.Game.Components.Add(this.player);

            this.player.TransitionToStateDying += this.OnTransitionToStateDying;
            this.player.TransitionToStateDead += this.OnTransitionToStateDead;
            this.player.TransitionToStateRespawn += this.OnTransitionToStateRespawn;
            this.player.TransitionToStateAlive += this.OnTransitionToStateAlive;
            this.player.HealthChanged += this.OnHealthChanged;
            this.player.Weapon.WeaponFired += this.OnWeaponFired;

            base.Initialize();
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

        private void OnTransitionToStateDying(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.audioService.PlaySound("explosion");

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

        private void OnWeaponFired(object sender, EventArgs e)
        {
            this.audioService.PlaySound("shot");
        }
    }
}
