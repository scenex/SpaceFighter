// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations.Enemies;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyService : GameComponent, IEnemyService
    {
        readonly IList<IEnemy> enemies = new List<IEnemy>();
        private IEnemyWeaponService enemyWeaponService;

        public EnemyService(Game game) : base(game)
        {
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return this.enemies;
            }
        }

        public IEnumerable<IShot> Shots
        {
            get
            {
                return this.enemyWeaponService.Weapon.Shots;
            }
        }

        public override void Initialize()
        {
            this.enemyWeaponService = (IEnemyWeaponService)this.Game.Services.GetService(typeof(IEnemyWeaponService));

            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(400, 400)));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //foreach (var enemy in this.enemies.ToList())
            //{
            //    if (enemy.WeaponTriggers.Any())
            //    {
            //        if(enemy.WeaponTriggers.First() < gameTime.TotalGameTime)
            //        {
            //            enemy.WeaponTriggers.Dequeue();
            //            this.enemyWeaponService.FireWeapon(new Vector2(enemy.Position.X, enemy.Position.Y), enemy.Height / 2, enemy.AngleToPlayer);
            //        }
            //    }
            //}

            foreach (var enemy in this.enemies.ToList().Where(enemy => enemy.IsAlive == false))
            {
                this.enemies.Remove(enemy);
            }

            base.Update(gameTime);
        }

        public void ReportEnemyHit(IEnemy enemy, IShot shot)
        {
            enemy.SubtractHealth(shot.FirePower);
        }

        public void RemoveShot(IShot shot)
        {
            this.enemyWeaponService.Weapon.Shots.Remove(shot);
        }
    }
}
