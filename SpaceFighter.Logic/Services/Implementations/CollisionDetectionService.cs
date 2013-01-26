// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CollisionDetectionService : GameComponent, ICollisionDetectionService
    {
        private IPlayerService playerService;
        private IEnemyService enemyService;
        private IWorldService worldService;

        private int levelHeight;
        private int levelWidth;

        private bool isCollisionWithBoundaries;
        private bool isCollisionDetectionActive;

        public CollisionDetectionService(Game game) : base(game)
        {
        }

        public event EventHandler<EventArgs> PlayerEnemyHit;
        public event EventHandler<EnemyHitEventArgs> EnemyHit;
        public event EventHandler<PlayerHitEventArgs> PlayerHit;
        public event EventHandler<EventArgs> BoundaryHit;

        public void Enable()
        {
            this.isCollisionDetectionActive = true;
        }

        public void Disable()
        {
            this.isCollisionDetectionActive = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
            this.enemyService = (IEnemyService)this.Game.Services.GetService(typeof(IEnemyService));
            this.worldService = (IWorldService)this.Game.Services.GetService(typeof(IWorldService));

            this.levelHeight = this.worldService.LevelHeight;
            this.levelWidth = this.worldService.LevelWidth;

            this.isCollisionDetectionActive = true;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (this.isCollisionDetectionActive)
            {
                this.CheckForCollisionBetweenPlayerAndEnemies();
                this.CheckForCollisionsBetweenPlayersShotsAndEnemies();
                this.CheckForCollisionsBetweenEnemiesShotsAndPlayer();
                this.CheckForCollisionsWithBoundaries();               
            }

            this.UpdateEnemyAngleToPlayer(); // Todo: Consider if this calculation is done at the right place
            base.Update(gameTime);
        }

        private void UpdateEnemyAngleToPlayer()
        {
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                var angle = Math.Atan2((enemy.Origin.Y - playerService.Player.Origin.Y), (enemy.Origin.X - playerService.Player.Origin.X)) + MathHelper.Pi; // Todo: why MathHelper.Pi?
                enemy.UpdateAngleToPlayer(angle);
            }
        }

        private void CheckForCollisionsBetweenEnemiesShotsAndPlayer()
        {
            // Check whether player was hit by a enemy's shot
            foreach (var shot in this.enemyService.Shots.ToList())
            {
                if (
                    this.IntersectPixels(
                        new Rectangle(
                            (int)this.playerService.Player.Position.X - this.playerService.Player.Width / 2, // Offset because we center player tile while drawing it
                            (int)this.playerService.Player.Position.Y - this.playerService.Player.Height / 2,
                            this.playerService.Player.Width,
                            this.playerService.Player.Height),
                        this.playerService.Player.ColorData,
                        new Rectangle((int)shot.Position.X, (int)shot.Position.Y, shot.Width, shot.Height),
                        shot.ColorData))
                {
                    if (this.PlayerHit != null)
                    {
                        this.PlayerHit(this, new PlayerHitEventArgs(shot));
                    }
                }
            }
        }

        private void CheckForCollisionsBetweenPlayersShotsAndEnemies()
        {
            // Check whether enemy was hit by a player's shot
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                foreach (var shot in this.playerService.Shots.ToList())
                {
                    if (this.IntersectPixels(new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Width, enemy.Height),
                                             enemy.ColorData,
                                             new Rectangle((int)shot.Position.X, (int)shot.Position.Y, shot.Width, shot.Height),
                                             shot.ColorData))
                    // PERFORMACE KILLER
                    //if (this.IntersectPixelsTranslated(
                    //    Matrix.CreateTranslation(new Vector3(enemy.Position, 0)), // Todo: Proper translation
                    //    enemy.Width,
                    //    enemy.Height,
                    //    enemy.ColorData,
                    //    Matrix.CreateTranslation(new Vector3(shot.Position, 0)),
                    //    shot.Width,
                    //    shot.Height,
                    //    shot.ColorData))

                    {
                        if (this.EnemyHit != null)
                        {
                            this.EnemyHit(this, new EnemyHitEventArgs(enemy, shot));
                        }
                    }
                }
            }
        }

        private void CheckForCollisionBetweenPlayerAndEnemies()
        {
            // Check for collisions between enemies and player
            foreach (var enemy in this.enemyService.Enemies)
            {
                if (this.IntersectPixels(
                    new Rectangle(
                        (int)this.playerService.Player.Position.X - this.playerService.Player.Width / 2,
                        (int)this.playerService.Player.Position.Y - this.playerService.Player.Height / 2, 
                        this.playerService.Player.Width, 
                        this.playerService.Player.Height),
                            this.playerService.Player.ColorData,
                                new Rectangle(
                                    (int)enemy.Position.X, 
                                    (int)enemy.Position.Y, 
                                    enemy.Width, 
                                    enemy.Height),
                                        enemy.ColorData))

                // PERFORMANCE KILLER
                //if (this.IntersectPixelsTranslated(
                //    Matrix.CreateTranslation(new Vector3(this.playerService.Player.Position, 0)),
                //    this.playerService.Player.Width,
                //    this.playerService.Player.Height,
                //    this.playerService.Player.ColorData,
                //    Matrix.CreateTranslation(new Vector3(enemy.Position, 0)), Todo: Proper translation
                //    enemy.Width,
                //    enemy.Height,
                //    enemy.ColorData))
                {
                    if (this.PlayerEnemyHit != null)
                    {
                        this.PlayerEnemyHit(this, null);
                    }
                }
            }
        }

        private void CheckForCollisionsWithBoundaries()
        {
            // Left
            if (this.playerService.Player.Position.X - (float)this.playerService.Player.Width / 2 <= 0)
            {
                this.isCollisionWithBoundaries = true;
            }

            // Right
            if (this.playerService.Player.Position.X + (float)this.playerService.Player.Width / 2 >= levelWidth)
            {
                this.isCollisionWithBoundaries = true;
            }

            // Top
            if (this.playerService.Player.Position.Y - (float)this.playerService.Player.Height / 2 <= 0)
            {
                this.isCollisionWithBoundaries = true;
            }

            // Bottom
            if (this.playerService.Player.Position.Y + (float)this.playerService.Player.Height / 2 >= levelHeight)
            {
                this.isCollisionWithBoundaries = true;
            }

            if (this.isCollisionWithBoundaries && this.BoundaryHit != null)
            {
                this.isCollisionWithBoundaries = false;
                this.BoundaryHit(this, null);
            }
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        private bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        private bool IntersectPixelsTranslated(
                    Matrix transformA, int widthA, int heightA, Color[] dataA,
                    Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
    }
}
