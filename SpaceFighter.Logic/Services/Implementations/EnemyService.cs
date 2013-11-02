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
        private readonly IEnemyFactory enemyFactory;
        private readonly List<TimeTriggerActionInvoker> enemiesTimeTriggerActionInvokers = new List<TimeTriggerActionInvoker>();

        private double elapsedTime;

        public EnemyService(Game game, IEnemyFactory enemyFactory) : base(game)
        {
            this.enemyFactory = enemyFactory;
        }

        public bool IsBossEliminated { get; private set; }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return new List<IEnemy>(Game.Components.OfType<IEnemy>());
            }
        }

        public IEnumerable<IShot> Shots
        {
            get
            {
                return this.Game.Components.OfType<IEnemy>().SelectMany(enemy => enemy.Weapon.Shots);
            }
        }

        public void SpawnEnemies()
        {
            this.IsBossEliminated = false;
            //this.enemyFactory.CreateAutonomous<EnemyAutonomous>(new Vector2(400, 400), true);      

            var waypoints = new Queue<Vector2>();
            waypoints.Enqueue(new Vector2(-80,  500));
            waypoints.Enqueue(new Vector2(0  ,  500));
            waypoints.Enqueue(new Vector2(80 ,  580));
            waypoints.Enqueue(new Vector2(160,  580));
            waypoints.Enqueue(new Vector2(240,  580));
            waypoints.Enqueue(new Vector2(320,  580));
            waypoints.Enqueue(new Vector2(400,  580));
            waypoints.Enqueue(new Vector2(480,  500));
            waypoints.Enqueue(new Vector2(560,  500));
            waypoints.Enqueue(new Vector2(640,  500));
            waypoints.Enqueue(new Vector2(720,  420));
            waypoints.Enqueue(new Vector2(800,  420));
            waypoints.Enqueue(new Vector2(880,  420));
            waypoints.Enqueue(new Vector2(960,  420));
            waypoints.Enqueue(new Vector2(1040, 420));
            this.enemiesTimeTriggerActionInvokers.Add(new TimeTriggerActionInvoker(1000, () => this.enemyFactory.CreateScripted<EnemyScripted>(waypoints, false)));


            var waypoints2 = new Queue<Vector2>();
            waypoints2.Enqueue(new Vector2(-80, 500));
            waypoints2.Enqueue(new Vector2(0, 500));
            waypoints2.Enqueue(new Vector2(80, 580));
            waypoints2.Enqueue(new Vector2(160, 580));
            waypoints2.Enqueue(new Vector2(240, 580));
            waypoints2.Enqueue(new Vector2(320, 580));
            waypoints2.Enqueue(new Vector2(400, 580));
            waypoints2.Enqueue(new Vector2(480, 500));
            waypoints2.Enqueue(new Vector2(560, 500));
            waypoints2.Enqueue(new Vector2(640, 500));
            waypoints2.Enqueue(new Vector2(720, 420));
            waypoints2.Enqueue(new Vector2(800, 420));
            waypoints2.Enqueue(new Vector2(880, 420));
            waypoints2.Enqueue(new Vector2(960, 420));
            waypoints2.Enqueue(new Vector2(1040, 420));
            this.enemiesTimeTriggerActionInvokers.Add(new TimeTriggerActionInvoker(4000, () => this.enemyFactory.CreateScripted<EnemyScripted>(waypoints2, true)));

            this.elapsedTime = 0;
        }

        public void UnspawnEnemies()
        {
            this.enemiesTimeTriggerActionInvokers.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            this.EnemySpawnTrigger(gameTime);
            base.Update(gameTime);
        }

        public void ReportEnemyHit(IEnemy enemy, IShot shot)
        {
            enemy.SubtractHealth(shot.FirePower);

            // Todo: Ugly..
            if (enemy.IsBoss && enemy.Health <= 0)
            {
                this.IsBossEliminated = true;
            }
        }

        public void RemoveShot(IShot shot)
        {
            var enemies = this.Game.Components.OfType<IEnemy>();
            foreach (var enemy in enemies.Where(enemy => enemy.Weapon.Shots.Contains(shot)))
            {
                enemy.Weapon.Shots.Remove(shot);
            }
        }

        private void EnemySpawnTrigger(GameTime gameTime)
        {
            foreach (var enemyTimeTriggerActionInvoker in this.enemiesTimeTriggerActionInvokers.ToList())
            {
                if (enemyTimeTriggerActionInvoker.TriggerMilliseconds <= this.elapsedTime)
                {
                    this.enemiesTimeTriggerActionInvokers.Remove(enemyTimeTriggerActionInvoker);
                    enemyTimeTriggerActionInvoker.TriggerAction();

                }
            }
        }
    }
}
