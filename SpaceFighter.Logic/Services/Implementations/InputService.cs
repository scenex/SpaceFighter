// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using SpaceFighter.Logic.Input.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class InputService : GameComponent, IInputService
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState currentGamePadState;

        private IPlayerService playerService;
        private IInput input;

        private bool isInputDeviceActive;

        public InputService(Game game, IPlayerService playerService) : base(game)
        {
            this.playerService = playerService;
        }

        public override void Initialize()
        {
            this.isInputDeviceActive = true;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(this.isInputDeviceActive)
            {
                if (this.input.DeviceType == typeof(Keyboard))
                {
                    this.ProcessInputKeyboard();
                }
                else if (this.input.DeviceType == typeof(GamePad))
                {
                    this.ProcessInputGamepad();
                }
            }

            base.Update(gameTime);
        }

        public void SetInputDevice(IInput inputDevice)
        {
            this.input = inputDevice;
        }

        public bool IsGamePadConnected
        {
            get
            {
                this.currentGamePadState = this.currentGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
                return this.currentGamePadState.IsConnected;
            }
        }

        public void Disable()
        {
            this.isInputDeviceActive = false;
        }

        public void Enable()
        {
            this.isInputDeviceActive = true;
        }

        public Type InputDeviceType
        {
            get
            {
                return (this.input.DeviceType);
            }
        }

        private void ProcessInputKeyboard()
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                this.playerService.RotateLeft();
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                this.playerService.RotateRight();
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.LeftControl) && this.previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                this.playerService.Fire();
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.LeftAlt))
            {
                this.playerService.Thrust();
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.F1) && this.previousKeyboardState.IsKeyUp(Keys.F1))
            {
                this.playerService.Player.Weapon.DowngradeWeapon();
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.F2) && this.previousKeyboardState.IsKeyUp(Keys.F2))
            {
                this.playerService.Player.Weapon.UpgradeWeapon();
            }

            this.previousKeyboardState = this.currentKeyboardState;
        }

        private void ProcessInputGamepad()
        {
            this.currentGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

            if (Math.Abs(this.currentGamePadState.ThumbSticks.Left.X - 0) > 0.1f || Math.Abs(this.currentGamePadState.ThumbSticks.Left.Y - 0) > 0.1f)
            {
                // http://plasticsturgeon.com/2012/08/rotate-the-shortest-direction/

                var originalRotation = this.playerService.Player.Rotation;
                var targetRotation = ((float)Math.Atan2(this.currentGamePadState.ThumbSticks.Left.Y, this.currentGamePadState.ThumbSticks.Left.X)) * -1; // Todo: Why do have to invert?
                var rotationDifference = (float)Math.Atan2(Math.Sin(targetRotation - originalRotation), Math.Cos(targetRotation - originalRotation));
                this.playerService.Player.SetRotation(rotationDifference * 0.05f);

                this.playerService.Thrust();
            }

            if (Math.Abs(this.currentGamePadState.ThumbSticks.Right.X - 0) > 0.1f || Math.Abs(this.currentGamePadState.ThumbSticks.Right.Y - 0) > 0.1f)
            {
                var originalRotation = this.playerService.Player.Weapon.Rotation;
                var targetRotation = ((float)Math.Atan2(this.currentGamePadState.ThumbSticks.Right.Y, this.currentGamePadState.ThumbSticks.Right.X)) * -1; // Todo: Why do have to invert?
                var rotationDifference = (float)Math.Atan2(Math.Sin(targetRotation - originalRotation), Math.Cos(targetRotation - originalRotation));
                this.playerService.Player.Weapon.Rotation += rotationDifference * 0.05f;

                this.playerService.Fire();
            }

            if (this.currentGamePadState.Triggers.Left > 0.1f)
            {
                this.playerService.Player.Weapon.DowngradeWeapon();
            }

            if (this.currentGamePadState.Triggers.Right > 0.1f)
            {
                this.playerService.Player.Weapon.UpgradeWeapon();
            }
        }
    }
}
