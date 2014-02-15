// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities;
    using SpaceFighter.Logic.Services.Interfaces;

    public class TerrainService : DrawableGameComponent, ITerrainService
    {
        private SpriteBatch spriteBatch;

        private Texture2D starSmall;
        private Texture2D starMedium;
        private Texture2D starLarge;
        
        private readonly List<Star> starfield = new List<Star>();
        private readonly Random random = new Random(5);

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

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            for (var i = 0; i < 70;i++)
            {
                this.starfield.Add(
                    new Star(
                        new Vector2(random.Next(this.LevelWidth), random.Next(this.LevelHeight)),
                        random.Next(1, 4)));
            }
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.starSmall = this.Game.Content.Load<Texture2D>(@"Sprites/Stars/Small");
            this.starMedium = this.Game.Content.Load<Texture2D>(@"Sprites/Stars/Medium");
            this.starLarge = this.Game.Content.Load<Texture2D>(@"Sprites/Stars/Large");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var star in starfield)
            {
                star.Position = new Vector2(star.Position.X, star.Position.Y + star.Velocity);

                if (star.Position.Y > this.LevelHeight)
                {
                    star.Position = new Vector2(random.Next(this.LevelWidth), 0);
                    star.Size = random.Next(1, 4);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            
            foreach (var star in starfield)
            {
                this.spriteBatch.Draw(this.MapStarSize(star.Size), star.Position, Color.White);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        private Texture2D MapStarSize(int starSize)
        {
            switch (starSize)
            {
                case 1:
                    return this.starSmall;
                case 2:
                    return this.starMedium;
                case 3:
                    return this.starLarge;
                default:
                    return this.starSmall;
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