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

        private const float ThrustIncrement = 0.2f;
        private const float ThrustFriction = 0.05f;
        private const float ThrustMax = 3.0f;

        private float thrustTotal;

        private bool isAfterGlow;

        private Vector2 previousPlayerPosition;

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
            get { return this.player; }
        }

        public IEnumerable<IShot> Shots
        {
            get { return this.player.Weapon.Shots; }
        }

        private bool IsMoveUpPossible
        {
            get { return this.player.Position.Y - (this.player.Height / 2.0) >= 0; }
        }

        private bool IsMoveDownPossible
        {
            get { return this.player.Position.Y + (this.player.Height / 2.0) <= this.terrainService.LevelHeight; }
        }

        private bool IsMoveLeftPossible
        {
            get { return this.player.Position.X - (this.player.Width / 2.0) >= 0; }
        }

        private bool IsMoveRightPossible
        {
            get { return this.player.Position.X + (this.player.Width / 2.0) <= this.terrainService.LevelWidth; }
        }

        public override void Update(GameTime gameTime)
        {
            this.thrustTotal = MathHelper.Clamp(this.thrustTotal -= ThrustFriction, 0.0f, ThrustMax);

            if (isAfterGlow)
            {
                this.TranslateShip();
            }

            this.isAfterGlow = true;
            base.Update(gameTime);
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

        public void Thrust(float angleDelta)
        {
            this.AccumulateThrust();
            this.TranslateShip();
        }

        public void MoveUp()
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.PiOver2 * (-1));
            this.AccumulateThrust();
            this.TranslateShip();
        }

        public void MoveDown()
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.PiOver2);
            this.AccumulateThrust();
            this.TranslateShip();
        }

        public void MoveLeft()
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.Pi);
            this.AccumulateThrust();
            this.TranslateShip();
        }

        public void MoveRight()
        {
            this.isAfterGlow = false;
            this.player.SetRotation(0);
            this.AccumulateThrust();
            this.TranslateShip();
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

        private void AccumulateThrust()
        {
            this.thrustTotal += ThrustIncrement;
        }

        private void TranslateShip()
        {
            this.player.Position = Vector2.Add(
                this.player.Position,
                new Vector2(
                    (float)Math.Cos(this.player.Rotation) * this.thrustTotal,
                    (float)Math.Sin(this.player.Rotation) * this.thrustTotal));

            if (!this.IsMoveLeftPossible || !this.IsMoveRightPossible)
            {
                this.player.Position = new Vector2(
                    this.previousPlayerPosition.X, 
                    this.player.Position.Y);
            }

            if (!this.IsMoveUpPossible || !this.IsMoveDownPossible)
            {
                this.player.Position = new Vector2(
                    this.player.Position.X,
                    this.previousPlayerPosition.Y);
            }

            this.previousPlayerPosition = this.player.Position;
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
