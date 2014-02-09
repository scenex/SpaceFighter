// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Console
{
    using System;
    using System.Diagnostics;

    using KeyboardHookTest;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.GamerServices;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using SpaceFighter.Interfaces;

    public class ConsoleService : DrawableGameComponent, IConsoleService
    {
        private SpriteBatch spriteBatch;

        private SpriteFont spriteFont;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private readonly ISpaceFighterApi spaceFighterApi;

        private string textInput;

        private bool isActive;

        private string typedText = "";

        public ConsoleService(Game game) : base(game)
        {
            this.isActive = true;
            this.spaceFighterApi = new SpaceFighterApi();
        }

        public override void Initialize()
        {
            //Append characters to the typedText string when the player types stuff on the keyboard.
            KeyGrabber.InboundCharEvent += (inboundCharacter) =>
            {
                if (inboundCharacter == 13)
                {
                    // Handle return
                }

                //Only append characters that exist in the spritefont.
                if (inboundCharacter < 32)
                    return;

                if (inboundCharacter > 126)
                    return;

                Debug.WriteLine(Char.GetNumericValue(inboundCharacter));

                typedText += inboundCharacter;
            };
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.currentKeyboardState = Keyboard.GetState();

            if (this.currentKeyboardState.IsKeyDown(Keys.Tab) && this.previousKeyboardState.IsKeyUp(Keys.Tab))
            {
                this.isActive = !this.isActive;
            }

            if (this.isActive)
            {

            }

            this.previousKeyboardState = this.currentKeyboardState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.isActive)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.DrawString(this.spriteFont, typedText, new Vector2(50, 50), Color.White);
                this.spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
}
