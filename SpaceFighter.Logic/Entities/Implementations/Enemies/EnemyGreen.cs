// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.AI;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyGreen : EnemyBase
    {
        private bool isAlive;
        private ISteering steeringStrategy;

        public EnemyGreen(Game game, Vector2 startPosition) : base(game, startPosition)
        {
            this.isAlive = true;
            this.Health = 100;

            this.steeringStrategy = new SteeringSeek();
        }

        public override bool IsAlive
        {
            get
            {
                return this.isAlive;
            }
        }

        public override void UpdatePosition()
        {
            this.Position = this.steeringStrategy.AdvancePosition(this.Position, this.distanceToPlayer, this.Rotation);
        }

        public override void InitializeStateMachine()
        {
            var alive = new State<Action<double>>(
                EnemyState.Alive,
                null,
                null,
                null);

            var dying = new State<Action<double>>(
                EnemyState.Dying,
                null,
                null,
                null);

            var dead = new State<Action<double>>(
                EnemyState.Dead,
                null,
                () => this.isAlive = false,
                null);

            alive.AddTransition(dying, () => this.Health <= 0);
            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        public override void LoadSprites()
        {
            this.spriteManager = new SpriteManager(EnemyState.Alive, 108, 128);

            this.spriteManager.AddStillSprite(
                EnemyState.Alive,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Alive"));

            this.spriteManager.AddAnimatedSprite(
                EnemyState.Dying,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Dying"));

            this.spriteManager.AddStillSprite(
                EnemyState.Dead,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Dead"));
        }
    }
}


