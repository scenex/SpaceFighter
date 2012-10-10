// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WeaponService : GameComponent, IWeaponService
    {
        private readonly Weapon weapon;

        public WeaponService(Game game) : base(game)
        {
            // TODO: Move into Initialize() or LoadContent()
            this.weapon = new Weapon(game);
            game.Components.Add(this.weapon);
        }

        public IWeapon Weapon
        {
            get
            {
                return this.weapon;
            }
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
