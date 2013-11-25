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
        private readonly PrimitiveBatch primitiveBatch;

        public HeadUpDisplayService(Game game) : base(game)
        {
            this.Health = 100;
            this.primitiveBatch = new PrimitiveBatch(game.GraphicsDevice);
        }

        public int Health { get; set; }

        public override void Draw(GameTime gameTime)
        {
            this.DrawEnergyBar();
            base.Draw(gameTime);
        }

        public void DrawEnergyBar()
        {
            this.primitiveBatch.Begin(PrimitiveType.LineList);

            this.primitiveBatch.AddVertex(new Vector2(95, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(205, 595), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(95, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(95, 615), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(205, 595), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(205, 615), Color.White);

            this.primitiveBatch.AddVertex(new Vector2(95, 615), Color.White);
            this.primitiveBatch.AddVertex(new Vector2(205, 615), Color.White);

            for (int i = 0; i < this.Health; i++)
            {
                this.primitiveBatch.AddVertex(new Vector2(100 + i, 600), Color.White);
                this.primitiveBatch.AddVertex(new Vector2(100 + i, 610), Color.White);
            }

            this.primitiveBatch.End();
        }        
    }
}
