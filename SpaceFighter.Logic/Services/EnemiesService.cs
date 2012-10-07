// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class EnemiesService : GameComponent, IEnemiesService
    {
        private Enemy enemy;

        public EnemiesService(Game game) : base(game)
        {
            this.enemy = new Enemy(game, new Vector2((640 / 2) - 16, 480 / 2)); // Todo: Get screen width and height from graphics service
            game.Components.Add(this.enemy);
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return new List<IEnemy>() {enemy};
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
