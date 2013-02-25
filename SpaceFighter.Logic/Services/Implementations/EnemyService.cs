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
                var shots = new List<IShot>();
                foreach (var enemy in this.enemies)
                {
                    shots.AddRange(enemy.Weapon.Shots);
                }

                return shots;
            }
        }

        public override void Initialize()
        {
            this.enemies.Add(EnemyFactory.Create<EnemyGreen>(this.Game, new Vector2(400, 400)));

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
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
            // Todo ! 
            this.enemies[0].Weapon.Shots.Remove(shot);
        }
    }
}
