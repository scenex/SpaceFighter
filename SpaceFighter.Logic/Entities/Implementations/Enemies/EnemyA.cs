// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Behaviours.Implementations;
    using SpaceFighter.Logic.Behaviours.Interfaces;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Implementations.WeaponStrategies;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyA : EnemyBase
    {
        private Weapon weapon;
        private IWeaponStrategy shootingStrategy;

        private IBehaviourStrategy behaviourStrategy;
        private readonly BehaviourStrategySeek behaviourStrategySeek;
        private readonly BehaviourStrategyFlee behaviourStrategyFlee;

        private Vector2 targetPosition;

        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public EnemyA(Game game, IPathFindingService pathFindingService, Vector2 startPosition) : base(game, pathFindingService, startPosition)
        {
            this.Health = 100;

            this.behaviourStrategySeek = new BehaviourStrategySeek();
            this.behaviourStrategyFlee = new BehaviourStrategyFlee();
        }

        public override void Initialize()
        {
            this.weapon = new WeaponEnemyA(this.Game);
            this.Game.Components.Add(this.weapon);

            this.waypoints = this.PathFindingService.GetPathToRandomTile(this.Position);

            base.Initialize();
        }

        public override IWeapon Weapon
        {
            get
            {
                return this.weapon;
            }
        }

        public override Queue<Vector2> Waypoints
        {
            get
            {
                return this.waypoints;
            }
        }

        protected override void UpdatePosition()
        {
            if (this.behaviourStrategy != null)
            {
                this.Position = this.behaviourStrategy.Execute(this.Position, this.targetPosition);

                // Target position reached?
                if (new Vector2(this.targetPosition.X - this.Position.X, this.targetPosition.Y - this.Position.Y).Length() < 40) // Todo: Magic number -> TileSize: 80 / 2 = 40
                {
                    if (this.waypoints.Count == 0)
                    {
                        while (this.waypoints.Count == 0)
                        {
                            this.waypoints = this.PathFindingService.GetPathToRandomTile(this.Position);
                        }
                    }
                    else
                    {
                        this.waypoints.Dequeue();
                        while (this.waypoints.Count == 0)
                        {
                            this.waypoints = this.PathFindingService.GetPathToRandomTile(this.Position);
                        }
                    }
                }
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
                delegate { this.targetPosition = this.waypoints.Peek(); },
                delegate
                    {
                        this.behaviourStrategy = this.behaviourStrategySeek;
                        this.shootingStrategy = null;

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },             
                null);

            var attack = new State<Action<double>>(
                EnemyState.Attack,
                delegate { this.targetPosition = this.PlayerPosition; },
                delegate
                    {
                        this.behaviourStrategy = this.behaviourStrategySeek;
                        this.shootingStrategy = new WeaponStrategyEnemyA();

                        this.IsHealthAdded = false;
                        this.IsHealthSubtracted = false;
                    },  
                null);

            var retreat = new State<Action<double>>(
                EnemyState.Retreat,
                delegate { this.targetPosition = this.PlayerPosition; },
                delegate
                    {
                        this.behaviourStrategy = this.behaviourStrategyFlee;
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

            patrol.AddTransition(attack, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() < 200);
            patrol.AddTransition(retreat, () => this.IsHealthSubtracted);
            patrol.AddTransition(dying, () => this.Health <= 0);

            attack.AddTransition(retreat, () => this.IsHealthSubtracted);
            attack.AddTransition(patrol, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() > 200);
            attack.AddTransition(dying, () => this.Health <= 0);

            //retreat.AddTransition(attack, () => ...); <- Should that be even possible?
            retreat.AddTransition(patrol, () => new Vector2(this.PlayerPosition.X - this.Position.X, this.PlayerPosition.Y - this.Position.Y).Length() > 200);
            retreat.AddTransition(dying, () => this.Health <= 0);

            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadSprites()
        {
            this.spriteManager = new SpriteManager(EnemyState.Alive, 80, 80);

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


