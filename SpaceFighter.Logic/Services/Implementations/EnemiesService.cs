// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemiesService : GameComponent, IEnemiesService
    {
        private Enemy enemy;

        public EnemiesService(Game game) : base(game)
        {
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return new List<IEnemy>() {this.enemy};
            }
        }

        public override void Initialize()
        {
            this.enemy = new Enemy(this.Game, new Vector2(100, 100));
            this.Game.Components.Add(this.enemy);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
