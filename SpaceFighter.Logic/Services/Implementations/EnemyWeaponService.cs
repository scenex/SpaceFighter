﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class EnemyWeaponService : GameComponent, IEnemyWeaponService
    {
        private Weapon weapon;

        public EnemyWeaponService(Game game) : base(game)
        {
        }

        public IWeapon Weapon
        {
            get
            {
                return this.weapon;
            }
        }

        public override void Initialize()
        {
            this.weapon = new EnemyWeapon(this.Game);
            this.Game.Components.Add(this.weapon);

            base.Initialize();
        }

        public void FireWeapon(Vector2 initialCoordinates, double angle)
        {
            this.weapon.FireWeapon(initialCoordinates, angle);
        }
    }
}