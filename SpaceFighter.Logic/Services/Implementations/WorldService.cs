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
        private int tileSize = 80;
        private readonly int[,] tileMap;
        private readonly int horizontalTileCount;
        private readonly int verticalTileCount;
        private SpriteBatch spriteBatch;

        private ICameraService cameraService;
        private IAudioService audioService;
        private readonly List<Texture2D> spriteList = new List<Texture2D>();

        public WorldService(Game game) : base(game)
        {
            this.tileMap = new[,]
                {
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x01, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x04, 0x05, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x05, 0x05, 0x06, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x07, 0x08, 0x08, 0x09, 0x00, 0x05 },
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 }
                };

            verticalTileCount = tileMap.GetUpperBound(0) + 1;
            horizontalTileCount = tileMap.GetUpperBound(1) + 1;
        }

        public override void Initialize()
        {
            this.cameraService = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));
            this.audioService = (IAudioService)this.Game.Services.GetService(typeof(IAudioService));
            
            this.LoadWorld();
            base.Initialize();
        }

        public int TileSize
        {
            get
            {
                return this.tileSize;
            }
        }

        public int[,] Map
        {
            get
            {
                return this.tileMap;
            }
        }

        public int LevelWidth
        {
            get
            {
                return this.horizontalTileCount * TileSize;
            }
        }

        public int LevelHeight
        {
            get
            {
                return this.verticalTileCount * TileSize;
            }
        }

        public void LoadWorld()
        {
            // DISABLE MUSIC WHILE DEVELOPMENT
            // this.audioService.PlaySound("music2");
        }

        public IEnumerable<int> GetCollidableTileIndices()
        {
            var collidableTileIndices = new List<int>();

            for (var i = 0; i < this.verticalTileCount; i++)
            {
                for (var j = 0; j < this.horizontalTileCount; j++)
                {
                    if (this.tileMap[i,j] != 0)
                    {
                        collidableTileIndices.Add(i * horizontalTileCount + j);
                    }
                }
            }

            return collidableTileIndices;
        }

        public IEnumerable<int> GetNonCollidableTileIndices()
        {
            var nonCollidableTileIndices = new List<int>();

            for (var i = 0; i < this.verticalTileCount; i++)
            {
                for (var j = 0; j < this.horizontalTileCount; j++)
                {
                    if (this.tileMap[i, j] == 0)
                    {
                        nonCollidableTileIndices.Add(i * horizontalTileCount + j);
                    }
                }
            }

            return nonCollidableTileIndices;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            var tileList = this.Game.Content.Load<List<string>>("manifest").Where(x => x.StartsWith(@"Sprites\L1\")).ToList();

            foreach (var tile in tileList)
            {
                spriteList.Add(this.Game.Content.Load<Texture2D>(tile));
            }

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront, 
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                cameraService.GetTransformation());

                for (int i = 0; i < verticalTileCount; i++)
                {
                    for (int j = 0; j < horizontalTileCount; j++)
                    {
                        this.spriteBatch.Draw(this.spriteList[this.tileMap[i, j]], new Vector2(j * TileSize, i * TileSize), Color.White);
                    }
                }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}