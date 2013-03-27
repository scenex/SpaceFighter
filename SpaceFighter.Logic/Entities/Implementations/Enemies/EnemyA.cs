// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Implementations.SteeringStrategies;
    using SpaceFighter.Logic.Entities.Implementations.WeaponStrategies;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyA : EnemyBase
    {
        private ISteeringStrategy steeringStrategy;
        private SteeringStrategySeek steeringStrategySeek;
        private SteeringStrategyFlee steeringStrategyFlee;
        private SteeringStrategyWander steeringStrategyWander;

        private IWeaponStrategy shootingStrategy;

        private Weapon weapon;

        public EnemyA(Game game, Vector2 startPosition) : base(game, startPosition)
        {
            this.Health = 100;
            this.shootingStrategy = new WeaponStrategyEnemyA();

            this.steeringStrategySeek = new SteeringStrategySeek();
            this.steeringStrategyFlee = new SteeringStrategyFlee();
            this.steeringStrategyWander = new SteeringStrategyWander();

            this.steeringStrategy = this.steeringStrategyWander;           
        }

        public override void Initialize()
        {
            this.weapon = new WeaponEnemyA(this.Game);
            this.Game.Components.Add(this.weapon);

            base.Initialize();
        }

        public override IWeapon Weapon
        {
            get
            {
                return this.weapon;
            }
        }

        protected override void UpdatePosition()
        {
            this.Position = this.steeringStrategy.Execute(this.Position, this.distanceToPlayer);
        }

        protected override void UpdateWeapon(TimeSpan elapsed)
        {
            this.weapon.Position = this.Position;
            this.weapon.Rotation = this.Rotation;
            this.shootingStrategy.Execute(() => this.Weapon.FireWeapon(), elapsed);
        }

        protected override void InitializeStateMachine()
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
                () => this.Game.Components.Remove(this),
                null);

            alive.AddTransition(dying, () => this.Health <= 0);
            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadSprites()
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


