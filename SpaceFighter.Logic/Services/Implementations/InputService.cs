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
        private GamePadState previousGamePadState;

        private IPlayerService playerService;
        private IInput input;

        private bool isInputDeviceActive;

        private GameTime gameTime;

        public InputService(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
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

            this.gameTime = gameTime;

            base.Update(gameTime);
        }

        public void SetInputDevice(IInput inputDevice)
        {
            this.input = inputDevice;
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

            this.previousKeyboardState = this.currentKeyboardState;
        }

        private void ProcessInputGamepad()
        {
            this.currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (gameTime != null)
            {
                this.playerService.Player.Rotation +=
                    new Vector2(
                        this.currentGamePadState.ThumbSticks.Left.X * (float)gameTime.ElapsedGameTime.TotalSeconds,
                        this.currentGamePadState.ThumbSticks.Left.Y * (float)gameTime.ElapsedGameTime.TotalSeconds).Length();
            }

            if (this.currentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                this.playerService.Thrust();
            }

            if (this.currentGamePadState.Buttons.A == ButtonState.Pressed && this.previousGamePadState.Buttons.A == ButtonState.Released)
            {
                this.playerService.Fire();
            }

            this.previousGamePadState = this.currentGamePadState;
        }
    }
}
