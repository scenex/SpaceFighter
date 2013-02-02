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

            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(
                this.Game, 
                new List<Vector2>
                    {
                        new Vector2(150, 100), 
                        new Vector2(200, 200)
                    }));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var enemy in enemies.ToList())
            {
                if (enemy.WeaponTriggers.Any())
                {
                    if(enemy.WeaponTriggers.First() < gameTime.TotalGameTime)
                    {
                        enemy.WeaponTriggers.Dequeue();
                        this.enemyWeaponService.FireWeapon(new Vector2(enemy.Origin.X, enemy.Origin.Y), enemy.Height / 2, enemy.AngleToPlayer);
                    }
                }
            }
            base.Update(gameTime);
        }

        public void ReportEnemyHit(IEnemy enemy, IShot shot)
        {
            enemy.SubtractHealth(shot.FirePower);

            if(enemy.Health <= 0)
            {
                this.Game.Components.Remove(enemy as IGameComponent);
                this.enemies.Remove(enemy);             
            }
        }

        public void RemoveShot(IShot shot)
        {
            this.enemyWeaponService.Weapon.Shots.Remove(shot);
        }
    }
}
