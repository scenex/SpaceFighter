// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    using SpaceFighter.Logic.Entities.Implementations.Behaviours;
    using SpaceFighter.Logic.Entities.Implementations.WeaponStrategies;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyA : EnemyBase
    {
        private Weapon weapon;

        private IWeaponStrategy shootingStrategy;

        private IBehaviourStrategy behaviourStrategy;
        private readonly BehaviourStrategySeek behaviourStrategySeek;
        private readonly BehaviourStrategyFlee behaviourStrategyFlee;
        private readonly BehaviourStrategyWander behaviourStrategyWander;
        private readonly BehaviourStrategyPathfinding behaviourStrategyPathfinding;

        public EnemyA(Game game, Vector2 startPosition) : base(game, startPosition)
        {
            this.Health = 100;

            this.behaviourStrategySeek = new BehaviourStrategySeek();
            this.behaviourStrategyFlee = new BehaviourStrategyFlee();
            this.behaviourStrategyWander = new BehaviourStrategyWander();
            this.behaviourStrategyPathfinding = new BehaviourStrategyPathfinding();
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
            if (this.behaviourStrategy != null)
            {
                this.Position = this.behaviourStrategy.Execute(this.Position, this.PlayerPosition);    
            }
        }

        protected override void UpdateWeapon(TimeSpan elapsed)
        {
            this.weapon.Position = this.Position;
            this.weapon.Rotation = this.Rotation;

            if (this.shootingStrategy != null)
            {
                this.shootingStrategy.Execute(() => this.Weapon.FireWeapon(), elapsed);
            }
        }

        protected override void InitializeStateMachine()
        {
            var alive = new State<Action<double>>(
                EnemyState.Alive,
                null,
                null,
                null);

            var patrol = new State<Action<double>>(
                EnemyState.Patrol,
                null,
                delegate
                    {
                        //this.behaviourStrategy = this.behaviourStrategyWander;
                        this.behaviourStrategy = this.behaviourStrategyPathfinding;
                        this.shootingStrategy = null;

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },             
                null);

            var attack = new State<Action<double>>(
                EnemyState.Attack,
                null,
                delegate
                    {
                        //this.behaviourStrategy = this.behaviourStrategySeek;
                        this.behaviourStrategy = this.behaviourStrategyPathfinding;
                        this.shootingStrategy = new WeaponStrategyEnemyA();

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },  
                null);

            var retreat = new State<Action<double>>(
                EnemyState.Retreat,
                null,
                delegate
                    {
                        //this.behaviourStrategy = this.behaviourStrategyFlee;
                        this.behaviourStrategy = this.behaviourStrategyPathfinding;
                        this.shootingStrategy = null;

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },  
                null);

            var dying = new State<Action<double>>(
                EnemyState.Dying,
                null,
                delegate
                    {
                        this.behaviourStrategy = null;
                        this.shootingStrategy = null;

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },
                null);

            var dead = new State<Action<double>>(
                EnemyState.Dead,
                null,
                () => this.Game.Components.Remove(this),
                null);

            alive.AddTransition(patrol, () => true);

            patrol.AddTransition(attack, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() < 250);
            patrol.AddTransition(retreat, () => this.IsHealthSubtracted);
            patrol.AddTransition(dying, () => this.Health <= 0);

            attack.AddTransition(retreat, () => this.IsHealthSubtracted);
            attack.AddTransition(patrol, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() > 250);
            attack.AddTransition(dying, () => this.Health <= 0);

            //retreat.AddTransition(attack, () => ...); <- Should that be even possible?
            retreat.AddTransition(patrol, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() > 250);
            retreat.AddTransition(dying, () => this.Health <= 0);

            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadSprites()
        {
            this.spriteManager = new SpriteManager(EnemyState.Alive, 108, 128);

            this.spriteManager.AddStillSprite(
                EnemyState.Alive,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Alive"));

            this.spriteManager.AddStillSprite(
                EnemyState.Patrol,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Alive"));

            this.spriteManager.AddStillSprite(
                EnemyState.Attack,
                this.Game.Content.Load<Texture2D>("Sprites/Enemy/Alive"));

            this.spriteManager.AddStillSprite(
                EnemyState.Retreat,
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


