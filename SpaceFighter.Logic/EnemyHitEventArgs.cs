// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;

    using SpaceFighter.Logic.Entities.Interfaces;

    public class EnemyHitEventArgs : EventArgs
    {
        public IEnemy Enemy { get; private set; }

        public IShot Shot { get; private set; }

        public EnemyHitEventArgs(IEnemy enemy, IShot shot)
        {
            this.Enemy = enemy;
            this.Shot = shot;
        }
    }
}
