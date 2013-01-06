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

        public InputService(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.input.DeviceType == typeof(Keyboard))
            {
                this.ProcessInputKeyboard();
            }
            else if (this.input.DeviceType == typeof(GamePad))
            {
                this.ProcessInputGamepad();
            }

            base.Update(gameTime);
        }

        public void SetInputDevice(IInput inputDevice)
        {
            this.input = inputDevice;
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

            //// LEFT
            //if (this.currentGamePadState.ThumbSticks.Left.X < 0.0f)
            //{
            //    if (this.playerService.Player.Position.X >= 0)
            //    {
            //        this.playerService.MoveLeft();
            //    }
            //}

            //// RIGHT
            //if (this.currentGamePadState.ThumbSticks.Left.X > 0.0f)
            //{
            //    if (this.playerService.Player.Position.X + this.playerService.Player.Width <= screenWidth)
            //    {
            //        this.playerService.MoveRight();
            //    }
            //}

            //// UP
            //if (this.currentGamePadState.ThumbSticks.Left.Y > 0.0f)
            //{
            //    if (this.playerService.Player.Position.Y - 3 >= 0)
            //    {
            //        this.playerService.MoveUp();
            //    }
            //}

            //// DOWN
            //if (this.currentGamePadState.ThumbSticks.Left.Y < 0.0f)
            //{
            //    if (this.playerService.Player.Position.Y + this.playerService.Player.Height <= screenHeight)
            //    {
            //        this.playerService.MoveDown();
            //    }
            //}

            if (this.currentGamePadState.Buttons.A == ButtonState.Pressed && this.previousGamePadState.Buttons.A == ButtonState.Released)
            {
                this.playerService.Fire();
            }

            this.previousGamePadState = this.currentGamePadState;
        }
    }
}
