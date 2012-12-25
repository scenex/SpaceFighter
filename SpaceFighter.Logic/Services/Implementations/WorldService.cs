// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class WorldService : DrawableGameComponent, IWorldService
    {
        private const int TileSize = 80;
        private readonly int[,] tileMap;
        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;
        private SpriteBatch spriteBatch;

        private readonly List<Texture2D> spriteList = new List<Texture2D>();

        public WorldService(Game game) : base(game)
        {
            var screenWidth = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenHeight = ((IGraphicsDeviceService)this.Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice.PresentationParameters.BackBufferHeight;

            horizontalTileCount = screenWidth / TileSize;
            verticalTileCount = screenHeight / TileSize;

            this.tileMap = new[,]
                {
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x04, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x08, 0x08, 0x09, 0x00 },
                    { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                };
        }

        public void LoadWorld()
        {
            throw new NotImplementedException();
        }

        public void GetCollidableElements()
        {
            throw new NotImplementedException();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            var tileList = this.Game.Content.Load<List<string>>("manifest").Where(x => x.StartsWith(@"Sprites\L1")).ToList();

            foreach (var tile in tileList)
            {
                spriteList.Add(this.Game.Content.Load<Texture2D>(tile));
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();

            for (int i = 0; i < verticalTileCount; i++)
            {
                for (int j = 0; j < horizontalTileCount; j++)
                {
                    this.spriteBatch.Draw(this.spriteList[this.tileMap[i,j]], new Vector2(j * TileSize, i * TileSize), Color.White);
                }
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}