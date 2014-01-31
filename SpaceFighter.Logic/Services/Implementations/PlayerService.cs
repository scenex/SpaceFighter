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

        private readonly IInputService inputService;

        private readonly IAudioService audioService;
        private readonly IPlayerFactory playerFactory;
        private readonly ITerrainService terrainService;

        private const float ThrustIncrement = 0.2f;
        private const float ThrustFriction = 0.05f;
        private const float ThrustMax = 3.0f;

        private float thrustTotal;

        private bool isAfterGlow;

        private Vector2 previousPlayerPosition;

        public PlayerService(Game game, IInputService inputService, IAudioService audioService, IPlayerFactory playerFactory, ITerrainService terrainService) : base(game)
        {
            this.inputService = inputService;
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
            this.UnsubscribeInputNotifications();
            this.player.Dispose();
        }

        private void OnAnalogMove(object sender, GamePadStateEventArgs gamePadStateEventArgs)
        {
            // http://plasticsturgeon.com/2012/08/rotate-the-shortest-direction/

            var originalRotation = this.Player.Rotation;
            var targetRotation = ((float)Math.Atan2(gamePadStateEventArgs.GamePadState.ThumbSticks.Left.Y, gamePadStateEventArgs.GamePadState.ThumbSticks.Left.X)) * -1; // Todo: Why do have to invert?
            var rotationDifference = (float)Math.Atan2(Math.Sin(targetRotation - originalRotation), Math.Cos(targetRotation - originalRotation));
            this.Player.SetRotationDelta(rotationDifference * 0.05f);

            this.AccumulateThrust();
            this.TranslateShip();
        }

        private void OnAnalogFire(object sender, GamePadStateEventArgs gamePadStateEventArgs)
        {
            var originalRotation = this.Player.Weapon.Rotation;
            var targetRotation = ((float)Math.Atan2(gamePadStateEventArgs.GamePadState.ThumbSticks.Right.Y, gamePadStateEventArgs.GamePadState.ThumbSticks.Right.X)) * -1; // Todo: Why do have to invert?
            var rotationDifference = (float)Math.Atan2(Math.Sin(targetRotation - originalRotation), Math.Cos(targetRotation - originalRotation));
            this.Player.Weapon.Rotation += rotationDifference * 0.05f;

            this.player.Weapon.FireWeapon();
        }

        private void OnMoveUp(object sender, EventArgs eventArgs)
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.PiOver2 * (-1));
            this.AccumulateThrust();
            this.TranslateShip();
        }

        private void OnMoveDown(object sender, EventArgs eventArgs)
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.PiOver2);
            this.AccumulateThrust();
            this.TranslateShip();
        }

        private void OnMoveLeft(object sender, EventArgs eventArgs)
        {
            this.isAfterGlow = false;
            this.player.SetRotation(MathHelper.Pi);
            this.AccumulateThrust();
            this.TranslateShip();
        }

        private void OnMoveRight(object sender, EventArgs eventArgs)
        {
            this.isAfterGlow = false;
            this.player.SetRotation(0);
            this.AccumulateThrust();
            this.TranslateShip();
        }

        private void OnFire(object sender, EventArgs eventArgs)
        {
            this.player.Weapon.FireWeapon();
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
            this.UnsubscribeInputNotifications();
            this.audioService.PlaySound("explosion");

            if (this.ShipExploding != null)
            {
                this.ShipExploding(this, stateChangedEventArgs);
            }
        }

        private void OnShipRespawning(object sender, StateChangedEventArgs stateChangedEventArgs)
        {
            this.SubscribeInputNotifications();

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

        private void SubscribeInputNotifications()
        {
            this.inputService.AnalogMoveChanged += this.OnAnalogMove;
            this.inputService.AnalogFireChanged += this.OnAnalogFire;
            this.inputService.MoveUpChanged += this.OnMoveUp;
            this.inputService.MoveDownChanged += this.OnMoveDown;
            this.inputService.MoveLeftChanged += this.OnMoveLeft;
            this.inputService.MoveRightChanged += this.OnMoveRight;
            this.inputService.FireChanged += this.OnFire;
        }

        private void UnsubscribeInputNotifications()
        {
            this.inputService.AnalogMoveChanged -= this.OnAnalogMove;
            this.inputService.AnalogFireChanged -= this.OnAnalogFire;
            this.inputService.MoveUpChanged -= this.OnMoveUp;
            this.inputService.MoveDownChanged -= this.OnMoveDown;
            this.inputService.MoveLeftChanged -= this.OnMoveLeft;
            this.inputService.MoveRightChanged -= this.OnMoveRight;
            this.inputService.FireChanged -= this.OnFire;
        }
    }
}
