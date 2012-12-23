// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class WorldService : DrawableGameComponent, IWorldService
    {
        private const int TileSize = 80;

        private int[,] tileMap;

        public WorldService(Game game) : base(game)
        {
            var screenWidth = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenHeight = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferHeight;

            var horizontalTileCount = screenWidth / TileSize;
            var verticalTileCount = screenHeight / TileSize;
            this.tileMap = new int[horizontalTileCount, verticalTileCount];

            for (int i = 0; i < horizontalTileCount; i++)
            {
                for (int j = 0; j < verticalTileCount; j++)
                {
                    this.tileMap[i, j] = 0x01;
                }
            }
        }

        public void LoadWorld()
        {
            throw new NotImplementedException();
        }

        public void GetCollidableElements()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}