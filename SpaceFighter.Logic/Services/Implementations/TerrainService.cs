// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class TerrainService : DrawableGameComponent, ITerrainService
    {
        public TerrainService(Game game) : base(game)
        {
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
                // 12 * 80 = 960
                return this.HorizontalTileCount * TileSize;
            }
        }

        public int LevelHeight
        {
            get
            {
                // 9 * 80 = 720
                return this.VerticalTileCount * TileSize;
            }
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