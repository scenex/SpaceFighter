// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemiesService : GameComponent, IEnemiesService
    {
        readonly IList<IEnemy> enemies = new List<IEnemy>(); 

        public EnemiesService(Game game) : base(game)
        {
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return enemies;
            }
        }

        public override void Initialize()
        {
            this.enemies.Add(new Enemy(this.Game, new Vector2(50, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(100, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(150, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(200, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(250, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(300, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(350, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(400, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(450, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(500, 100)));
            this.enemies.Add(new Enemy(this.Game, new Vector2(550, 100)));

            base.Initialize();
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
