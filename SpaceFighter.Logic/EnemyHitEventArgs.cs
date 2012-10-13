// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    public class EnemyHitEventArgs : EventArgs
    {
        public IEnemy Enemy { get; private set; }

        public IWeapon Weapon { get; private set; }

        public EnemyHitEventArgs(IEnemy enemy, IWeapon weapon)
        {
            this.Enemy = enemy;
            this.Weapon = weapon;
        }
    }
}
