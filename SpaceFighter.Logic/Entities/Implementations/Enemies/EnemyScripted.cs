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
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyScripted : EnemyBase
    {
        private readonly ICameraService cameraService;

        private readonly Weapon weapon;
        
        private readonly IBehaviourStrategy behaviourStrategy;
        private IWeaponStrategy shootingStrategy;

        private readonly bool isBoss;
        private Vector2 targetPosition;
        private readonly Queue<Vector2> waypoints = new Queue<Vector2>();

        private bool isOffscreen;

        public EnemyScripted(Game game, ICameraService cameraService, Vector2 startPosition, bool isBoss) : base(game, cameraService, startPosition)
        {
            this.isBoss = isBoss;
            this.Health = 100;

            this.waypoints.Enqueue(new Vector2(400, 100));
            this.waypoints.Enqueue(new Vector2(700, 100));
            this.waypoints.Enqueue(new Vector2(700, 500));
            this.waypoints.Enqueue(new Vector2(400, 500));

            this.behaviourStrategy = new BehaviourStrategySeek();
            this.cameraService = cameraService;
            
            this.Game.Components.Add(this);

            this.weapon = new WeaponEnemyA(this.Game, this.cameraService); // Todo: Factory
            this.Game.Components.Add(this.weapon);
        }

        public override void Initialize()
        {
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
            if (this.Health > 0)
            {
                this.Position = this.behaviourStrategy.Execute(this.Position, this.targetPosition);

                // Target position reached?
                if (new Vector2(this.targetPosition.X - this.Position.X, this.targetPosition.Y - this.Position.Y).Length() < 40) // Todo: Magic number -> TileSize: 80 / 2 = 40
                {
                    if (this.waypoints.Count != 0)
                    {
                        this.waypoints.Dequeue();

                        if (this.waypoints.Count == 0)
                        {
                            this.Health = 0;
                            this.isOffscreen = true;
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

        public override bool IsBoss
        {
            get
            {
                return isBoss;
            }
        }

        protected override void InitializeStateMachine()
        {
            var alive = new State<Action<double>>(
                EnemyState.Alive,
                delegate
                    {
                        if (this.waypoints.Count != 0)
                        {
                            this.targetPosition = this.waypoints.Peek();
                        }
                    },
                null,
                null);

            var dying = new State<Action<double>>(
                EnemyState.Dying,
                null,
                delegate
                    {
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

            alive.AddTransition(dying, () => this.Health <= 0 && this.isOffscreen == false);
            alive.AddTransition(dead, () => this.Health >= 0 && this.isOffscreen);

            dying.AddTransition(dead, () => this.spriteManager.IsAnimationDone(this.stateMachine.CurrentState.Name));

            this.stateMachine = new StateMachine<Action<double>>(alive);
        }

        protected override void LoadSprites()
        {
            this.spriteManager = new SpriteManager(EnemyState.Alive, 80, 80);

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


