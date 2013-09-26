// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Implementations.WeaponStrategies;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;
    using SpaceFighter.Logic.StateMachine;

    public class EnemyScripted : EnemyBase
    {
        private readonly ICameraService cameraService;

        private readonly Weapon weapon;
        private IWeaponStrategy shootingStrategy;

        private readonly bool isBoss;
        private Vector2 targetPosition;
        private Queue<Vector2> waypoints = new Queue<Vector2>();

        public EnemyScripted(Game game, ICameraService cameraService, Vector2 startPosition, bool isBoss) : base(game, cameraService, startPosition)
        {
            this.isBoss = isBoss;
            this.Health = 100;

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
            // Todo Fix:
            this.Position = Vector2.CatmullRom(
                this.Position, 
                this.Position + new Vector2(1, 1), 
                this.Position + new Vector2(2, 3), 
                this.Position + new Vector2(4, 2), 0.1f);
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
            // Todo: redesign state machine for scripted enemies

            var alive = new State<Action<double>>(
                EnemyState.Alive,
                null,
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

            alive.AddTransition(dying, () => this.Health <= 0);
            //alive.AddTransition(dead, () => this.Health >= 0 && this.Position == outofscreen);

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


