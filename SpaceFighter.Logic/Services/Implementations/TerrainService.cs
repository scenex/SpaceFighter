// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Services.Interfaces;

    public class TerrainService : DrawableGameComponent, ITerrainService
    {
        private readonly ICameraService cameraService;

        private SpriteBatch spriteBatch;

        private Texture2D backgroundTexture;

        public TerrainService(Game game, ICameraService cameraService) : base(game)
        {
            this.cameraService = cameraService;
            this.TileSize = 80;

            this.Map = new[,]
                {
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x05, 0x05, 0x05, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x05, 0x05, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x05, 0x05, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x05, 0x05, 0x05, 0x00, 0x05 },
                    { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x05, 0x05, 0x05, 0x00, 0x05 },
                    { 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05 }
                };

            this.VerticalTileCount = this.Map.GetUpperBound(0) + 1;
            this.HorizontalTileCount = this.Map.GetUpperBound(1) + 1;
        }

        public int TileSize { get; private set; }
        public int[,] Map { get; private set; }
        public int VerticalTileCount { get; private set; }
        public int HorizontalTileCount { get; private set; }

        public int LevelWidth
        {
            get
            {
                return this.HorizontalTileCount * TileSize;
            }
        }

        public int LevelHeight
        {
            get
            {
                return this.VerticalTileCount * TileSize;
            }
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.backgroundTexture = new Texture2D(this.GraphicsDevice, LevelWidth, LevelHeight, false, SurfaceFormat.Color);
            
            var colors = new Color[LevelWidth * LevelHeight];

            // Borders
            for (var x = 0; x < LevelWidth; x++)
            {
                for (var y = 0; y < LevelHeight; y++)
                {
                    if (x == 0 || x == LevelWidth - 1)
                    {
                        colors[x + y * LevelWidth] = Color.White;
                    }
                }
            }
            
            this.backgroundTexture.SetData(colors);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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

            this.spriteBatch.Draw(this.backgroundTexture, new Vector2(0, 0), Color.White);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Old stuff

        //protected override void LoadContent()
        //{
        //    this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

        //    var tileList = this.Game.Content.Load<List<string>>("manifest").Where(x => x.StartsWith(@"Sprites\L1\")).ToList();

        //    foreach (var tile in tileList)
        //    {
        //        spriteList.Add(this.Game.Content.Load<Texture2D>(tile));
        //    }

        //    base.LoadContent();
        //}
        
        //public override void Draw(GameTime gameTime)
        //{
        //    // this.debugService.DrawRectangle(new Rectangle(((int)playerService.Player.Position.X / 80) * 80, ((int)playerService.Player.Position.Y / 80) * 80, 80, 80));
        //    this.spriteBatch.Begin(
        //        SpriteSortMode.BackToFront,
        //        BlendState.AlphaBlend,
        //        null,
        //        null,
        //        null,
        //        null,
        //        cameraService.GetTransformation());

        //    for (int i = 0; i < this.VerticalTileCount; i++)
        //    {
        //        for (int j = 0; j < this.HorizontalTileCount; j++)
        //        {
        //            this.spriteBatch.Draw(
        //                this.spriteList[this.Map[i, j]],
        //                new Vector2(j * this.TileSize, i * this.TileSize),
        //                Color.White);
        //        }
        //    }

        //    this.spriteBatch.End();

        //    base.Draw(gameTime);
        //}

        #endregion
    }
}