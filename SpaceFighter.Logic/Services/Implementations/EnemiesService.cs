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
            // TODO: Move into Initialize() or LoadContent()
            // this.enemy = new Enemy(game, new Vector2((640 / 2) - 16, 480 / 2)); // Todo: Get screen width and height from graphics service
            this.enemy = new Enemy(game, new Vector2(100, 100));
            game.Components.Add(this.enemy);
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                return new List<IEnemy>() {this.enemy};
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
