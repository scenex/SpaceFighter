// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Services.Interfaces;

    public class HeadUpDisplayService : DrawableGameComponent, IHeadUpDisplayService
    {
        private readonly Game game;

        private PrimitiveBatch primitiveBatch;

        public HeadUpDisplayService(Game game) : base(game)
        {
            this.game = game;
            this.Health = 100;
        }

        public int Health { get; set; }

        public override void Initialize()
        {   
            base.Initialize();
            this.primitiveBatch = new PrimitiveBatch(game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawEnergyBar();
            base.Draw(gameTime);
        }

        public void DrawEnergyBar()
        {
            this.primitiveBatch.Begin(PrimitiveType.LineList);

            this.primitiveBatch.AddVertex(new Vector2(95, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(305, 595), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(95, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(95, 625), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(305, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(305, 625), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(95, 625), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(305, 625), Color.White);

            for (int i = 0; i < this.Health * 2; i++)
            {
                this.primitiveBatch.AddVertex(new Vector2(100 + i, 600), Color.White);
                this.primitiveBatch.AddVertex(new Vector2(100 + i, 620), Color.White);
            }

            this.primitiveBatch.End();
        }        
    }
}
