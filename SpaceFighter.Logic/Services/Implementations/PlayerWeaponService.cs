// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Implementations.Weapons;
    using SpaceFighter.Logic.Entities.Interfaces;
    using SpaceFighter.Logic.Services.Interfaces;

    public class PlayerWeaponService : GameComponent, IPlayerWeaponService
    {
        private Weapon weapon;

        public PlayerWeaponService(Game game) : base(game)
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
            this.weapon = new PlayerWeapon(this.Game);
            this.Game.Components.Add(this.weapon);

            base.Initialize();
        }

        public void FireWeapon(Vector2 initialCoordinates)
        {
            this.weapon.FireWeapon(initialCoordinates);
        }

        public void UpgradeWeapon()
        {
        }

        public void DowngradeWeapon()
        {
        }

        public void RemoveShot(IShot shot)
        {
            this.weapon.Shots.Remove(shot);
        }
    }
}
