// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Screens
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class MenuScreen : DrawableGameComponent, IScreenTransition
    {
        private readonly IInputService inputService;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private Curve fadeInCurve;
        private Curve fadeOutCurve;
        private Curve currentFadeCurve;

        private const double TransitionTime = 1500;
        private double elapsedTime;
        private int selectionIndex;

        public MenuScreen(Game game, IInputService inputService) : base(game)
        {
            this.inputService = inputService;

            this.inputService.MenuSelectionUpChanged += delegate { this.selectionIndex--; };
            this.inputService.MenuSelectionDownChanged += delegate { this.selectionIndex++; };
            this.inputService.MenuSelectionConfirmedChanged += this.OnMenuSelectionConfirmedChanged;
        }

        public bool IsTransitionAllowed { get; private set; }
        public object TransitionTag { get; private set; }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.spriteFont = this.Game.Content.Load<SpriteFont>(@"DefaultFont");

            this.fadeInCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeIn");
            this.fadeOutCurve = this.Game.Content.Load<Curve>(@"Curves\MenuTextFadeOut");
            this.currentFadeCurve = fadeInCurve;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.TransitionTag != null && elapsedTime > TransitionTime)
            {
                this.IsTransitionAllowed = true;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            this.spriteBatch.DrawString(
                this.spriteFont, 
                "Start Game", 
                new Vector2(570, 500), 
                this.GetSelectionIndex() == 0
                ? Color.Red * this.currentFadeCurve.Evaluate((float)this.elapsedTime / 1000)
                : Color.White * this.currentFadeCurve.Evaluate((float)this.elapsedTime / 1000));

            this.spriteBatch.DrawString(
                this.spriteFont, 
                "Exit Game", 
                new Vector2(570, 550), 
                this.GetSelectionIndex() == 1
                ? Color.Red * this.currentFadeCurve.Evaluate((float)this.elapsedTime / 1000)
                : Color.White * this.currentFadeCurve.Evaluate((float)this.elapsedTime / 1000));
            
            this.spriteBatch.End();
        }

        private void OnMenuSelectionConfirmedChanged(object sender, EventArgs eventArgs)
        {
            if (this.GetSelectionIndex() == 0)
            {
                this.elapsedTime = 0;
                this.currentFadeCurve = fadeOutCurve;
                this.TransitionTag = MenuItems.StartGame;
                this.inputService.Disable();
            }

            if (this.GetSelectionIndex() == 1)
            {
                this.elapsedTime = 0;
                this.currentFadeCurve = fadeOutCurve;
                this.TransitionTag = MenuItems.ExitGame;
                this.inputService.Disable();
            }
        }

        private int GetSelectionIndex()
        {
            return Math.Abs(this.selectionIndex % 2);
        }
    }
}
