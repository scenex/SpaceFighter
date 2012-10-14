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
        private IEnemiesService enemyService;
        private IWeaponService weaponService;

        public CollisionDetectionService(Game game) : base(game)
        {
        }

        public event EventHandler<EventArgs> PlayerEnemyHit;

        public event EventHandler<EnemyHitEventArgs> EnemyHit;

        public event EventHandler<EventArgs> PlayerHit;

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            this.playerService = (IPlayerService)this.Game.Services.GetService(typeof(IPlayerService));
            this.enemyService = (IEnemiesService)this.Game.Services.GetService(typeof(IEnemiesService));
            this.weaponService = (IWeaponService)this.Game.Services.GetService(typeof(IWeaponService));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Check for collisions between enemies and player
            foreach (var enemy in this.enemyService.Enemies)
            {
                if (this.IntersectPixels(new Rectangle((int)this.playerService.Player.Position.X, (int)this.playerService.Player.Position.Y, this.playerService.Player.Width, this.playerService.Player.Height), 
                                         this.playerService.Player.ColorData,
                                         new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Width, enemy.Height), 
                                         enemy.ColorData))
                {
                    if(this.PlayerEnemyHit != null)
                    {
                        this.PlayerEnemyHit(this, null);
                    }
                }
            }

            // Check whether enemy was hit by a player's shot
            foreach (var enemy in this.enemyService.Enemies.ToList())
            {
                foreach (var shot in this.weaponService.Weapon.Shots.ToList())
                {  
                    if (this.IntersectPixels(new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Width, enemy.Height),
                                             enemy.ColorData,
                                             new Rectangle((int)shot.Position.X, (int)shot.Position.Y, shot.Width, shot.Height),
                                             shot.ColorData))
                    {
                        if (this.EnemyHit != null)
                        {
                            this.EnemyHit(this, new EnemyHitEventArgs(enemy, shot));
                        }
                    }
                }
            }

            base.Update(gameTime);
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
        public bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
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
    }
}
