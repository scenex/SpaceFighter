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

            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(50, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyRed>(this.Game, new Vector2(100, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(150, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyRed>(this.Game, new Vector2(200, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(250, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyRed>(this.Game, new Vector2(300, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(350, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyRed>(this.Game, new Vector2(400, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(450, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyRed>(this.Game, new Vector2(500, 100)));
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(550, 100)));

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
                        this.enemyWeaponService.FireWeapon(new Vector2(enemy.Position.X + ((float)enemy.Width / 2), enemy.Position.Y));
                    }
                }
            }
            base.Update(gameTime);
        }

        public void ReportEnemyHit(IEnemy enemy, IShot shot)
        {
            enemy.Energy -= shot.FirePower;

            if(enemy.Energy <= 0)
            {
                this.Game.Components.Remove(enemy as IGameComponent);
                this.enemies.Remove(enemy);             
            }
        }
    }
}
