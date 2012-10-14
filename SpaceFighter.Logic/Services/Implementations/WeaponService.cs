﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;

    using SpaceFighter.Logic.Entities.Implementations;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WeaponService : GameComponent, IWeaponService
    {
        private Weapon weapon;

        public WeaponService(Game game) : base(game)
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
            this.weapon = new Weapon(this.Game);
            this.Game.Components.Add(this.weapon);

            base.Initialize();
        }

        public void FireWeapon(Vector2 initialCoordinates)
        {
            this.weapon.FireWeapon(initialCoordinates);
        }

        public void UpgradeWeapon()
        {
            throw new System.NotImplementedException();
        }

        public void DowngradeWeapon()
        {
            throw new System.NotImplementedException();
        }
    }
}
