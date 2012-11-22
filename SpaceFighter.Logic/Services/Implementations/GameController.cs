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

    public class GameController : GameComponent, IGameController
    {
        private CollisionDetectionService collisionDetectionService;
        private PlayerService playerService;
        private EnemyService enemyService;
        private IInput input;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;

        private const int ScreenWidth = 640;
        private const int ScreenHeight = 480;

        public GameController(Game game) : base(game)
        {
        }

        public void UpdateInput(IInput input)
        {
            this.input = input;
        }

        public Type InputDeviceType
        {
            get
            {
                return (this.input.DeviceType);
            }
        }

        public void SetInputDevice(IInput inputDevice)
        {
            this.input = inputDevice;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.input.DeviceType == typeof(Keyboard))
            {
                this.ProcessInputKeyboard();
            }
            else 
            {
                this.ProcessInputGamepad();
            }

            base.Update(gameTime);
        }

        public override void Initialize()
        {
            this.collisionDetectionService = new CollisionDetectionService(this.Game);
            this.Game.Components.Add(this.collisionDetectionService);

            this.playerService = new PlayerService(this.Game);
            this.Game.Services.AddService(typeof(IPlayerService), this.playerService);
            this.Game.Components.Add(this.playerService);

            this.enemyService = new EnemyService(this.Game);
            this.Game.Services.AddService(typeof(IEnemyService), this.enemyService);
            this.Game.Components.Add(this.enemyService);

            this.collisionDetectionService.EnemyHit += this.OnEnemyHit;
            this.collisionDetectionService.PlayerHit += this.OnPlayerHit;
            
            base.Initialize();
        }

        private void OnPlayerHit(object sender, PlayerHitEventArgs e)
        {
            this.enemyService.RemoveShot(e.Shot);
            this.playerService.ReportPlayerHit(e.Shot);
        }

        private void OnEnemyHit(object sender, EnemyHitEventArgs e)
        {
            this.playerService.RemoveShot(e.Shot);
            this.enemyService.ReportEnemyHit(e.Enemy, e.Shot);         
        }

        private void ProcessInputKeyboard()
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Up))
            {
                if (this.playerService.Player.Position.Y - 3 >= 0)
                {
                    this.playerService.MoveUp();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Down))
            {
                if (this.playerService.Player.Position.Y + this.playerService.Player.Height <= ScreenHeight)
                {
                    this.playerService.MoveDown();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Left))
            {
                if (this.playerService.Player.Position.X >= 0)
                {
                    this.playerService.MoveLeft();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.Right))
            {
                if (this.playerService.Player.Position.X + this.playerService.Player.Width <= ScreenWidth)
                {
                    this.playerService.MoveRight();
                }
            }

            if (this.currentKeyboardState.IsKeyDown(Keys.LeftControl) && this.previousKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                this.playerService.Fire();
            }

            this.previousKeyboardState = this.currentKeyboardState;
        }

        private void ProcessInputGamepad()
        {
            //this.currentGamePadState = GamePad.GetState(PlayerIndex.Two);

            //if (this.currentGamePadState.(Keys.Up))
            //{
            //    if (this.playerService.Player.Position.Y - 3 >= 0)
            //    {
            //        this.playerService.MoveUp();
            //    }
            //}

            //if (this.currentGamePadState.IsKeyDown(Keys.Down))
            //{
            //    if (this.playerService.Player.Position.Y + this.playerService.Player.Height <= ScreenHeight)
            //    {
            //        this.playerService.MoveDown();
            //    }
            //}

            //if (this.currentGamePadState.IsKeyDown(Keys.Left))
            //{
            //    if (this.playerService.Player.Position.X >= 0)
            //    {
            //        this.playerService.MoveLeft();
            //    }
            //}

            //if (this.currentGamePadState.IsKeyDown(Keys.Right))
            //{
            //    if (this.playerService.Player.Position.X + this.playerService.Player.Width <= ScreenWidth)
            //    {
            //        this.playerService.MoveRight();
            //    }
            //}

            //if (this.currentGamePadState.IsKeyDown(Keys.LeftControl) && this.previousGamePadState.IsKeyUp(Keys.LeftControl))
            //{
            //    this.playerService.Fire();
            //}

            //this.previousGamePadState = this.currentGamePadState;
        }
    }
}
