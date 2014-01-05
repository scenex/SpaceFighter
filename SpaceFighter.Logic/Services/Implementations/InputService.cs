// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using SpaceFighter.Logic.Input.Implementation;
    using SpaceFighter.Logic.Input.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class InputService : GameComponent, IInputService
    {
        public event EventHandler<GamePadStateEventArgs> AnalogMoveChanged;
        public event EventHandler<GamePadStateEventArgs> AnalogFireChanged;
        public event EventHandler<GamePadStateEventArgs> AnalogPauseChanged;

        public event EventHandler MoveUpChanged;
        public event EventHandler MoveDownChanged;
        public event EventHandler MoveLeftChanged;
        public event EventHandler MoveRightChanged;
        public event EventHandler FireChanged;

        public event EventHandler PauseChanged;

        public event EventHandler MenuSelectionUpChanged;
        public event EventHandler MenuSelectionDownChanged;
        public event EventHandler MenuSelectionConfirmedChanged;

        private IInput input;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;

        private bool isInputDeviceActive;

        public InputService(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            #if WINDOWS
            if (IsGamePadConnected)
            {
                this.SetInputDevice(new InputGamepad());
            }
            else
            {
                this.SetInputDevice(new InputKeyboard());
            }
            #elif XBOX
                this.SetInputDevice(new InputGamepad());
            #endif
            
            this.Enable();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.isInputDeviceActive)
            {
                if (this.input.DeviceType == typeof(Keyboard))
                {
                    if (this.InputStateHandling == InputStateHandling.Gameplay)
                    {
                        this.ProcessInputKeyboardGameplay();
                    }

                    if (this.InputStateHandling == InputStateHandling.Menu)
                    {
                        this.ProcessInputKeyboardMenu();
                    }
                }
                else if (this.input.DeviceType == typeof(GamePad))
                {
                    if (this.InputStateHandling == InputStateHandling.Gameplay)
                    {
                        this.ProcessInputGamepadGameplay();
                    }

                    if (this.InputStateHandling == InputStateHandling.Menu)
                    {
                        this.ProcessInputGamepadMenu();
                    }
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

        public InputStateHandling InputStateHandling { get; set; }

        public bool IsGamePaused { get; set; }

        private void ProcessInputKeyboardGameplay()
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if(this.MoveLeftChanged != null)
                {
                    this.MoveLeftChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (this.MoveRightChanged != null)
                {
                    this.MoveRightChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (this.MoveUpChanged != null)
                {
                    this.MoveUpChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (this.MoveDownChanged != null)
                {
                    this.MoveDownChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.LeftControl) && this.previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                if (this.FireChanged != null)
                {
                    this.FireChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.P) && this.previousKeyboardState.IsKeyUp(Keys.P))
            {
                if (this.PauseChanged != null)
                {
                    this.PauseChanged(this, EventArgs.Empty);
                }
            }

            this.previousKeyboardState = this.currentKeyboardState;
        }

        private void ProcessInputGamepadGameplay()
        {
            this.currentGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

            if (Math.Abs(this.currentGamePadState.ThumbSticks.Left.X - 0) > 0.1f || Math.Abs(this.currentGamePadState.ThumbSticks.Left.Y - 0) > 0.1f)
            {
                if(this.AnalogMoveChanged != null)
                {
                    this.AnalogMoveChanged(this, new GamePadStateEventArgs(this.currentGamePadState));
                }
            }

            if (Math.Abs(this.currentGamePadState.ThumbSticks.Right.X - 0) > 0.1f || Math.Abs(this.currentGamePadState.ThumbSticks.Right.Y - 0) > 0.1f)
            {
                if (this.AnalogFireChanged != null)
                {
                    this.AnalogFireChanged(this, new GamePadStateEventArgs(this.currentGamePadState));
                }
            }

            if (this.previousGamePadState.Buttons.Start == ButtonState.Pressed && this.currentGamePadState.Buttons.Start == ButtonState.Released)
            {
                if (this.AnalogPauseChanged != null)
                {
                    this.AnalogPauseChanged(this, new GamePadStateEventArgs(this.currentGamePadState));
                }
            }

            this.previousGamePadState = this.currentGamePadState;
        }

        private void ProcessInputKeyboardMenu()
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Up) && this.previousKeyboardState.IsKeyUp(Keys.Up))
            {
                if(this.MenuSelectionUpChanged != null)
                {
                    this.MenuSelectionUpChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Down) && this.previousKeyboardState.IsKeyUp(Keys.Down))
            {
                if (this.MenuSelectionDownChanged != null)
                {
                    this.MenuSelectionDownChanged(this, EventArgs.Empty);
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Space) && this.previousKeyboardState.IsKeyUp(Keys.Space))
            {
                if (this.MenuSelectionConfirmedChanged != null)
                {
                    this.MenuSelectionConfirmedChanged(this, EventArgs.Empty);
                }
            }

            this.previousKeyboardState = this.currentKeyboardState;
        }

        private void ProcessInputGamepadMenu()
        {
            this.currentGamePadState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

            if (this.previousGamePadState.DPad.Up == ButtonState.Pressed && this.currentGamePadState.DPad.Up == ButtonState.Released)
            {
                if (this.MenuSelectionUpChanged != null)
                {
                    this.MenuSelectionUpChanged(this, EventArgs.Empty);
                }
            }

            if (this.previousGamePadState.DPad.Down == ButtonState.Pressed && this.currentGamePadState.DPad.Down == ButtonState.Released)
            {
                if (this.MenuSelectionDownChanged != null)
                {
                    this.MenuSelectionDownChanged(this, EventArgs.Empty);
                }
            }

            if (this.previousGamePadState.Buttons.A == ButtonState.Pressed)
            {
                if (this.MenuSelectionConfirmedChanged != null)
                {
                    this.MenuSelectionConfirmedChanged(this, EventArgs.Empty);
                }
            }

            this.previousGamePadState = this.currentGamePadState;
        }
    }

    public enum InputStateHandling
    {
        Menu,
        Gameplay
    }
}
