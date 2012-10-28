// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Weapons
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class EnemyWeapon : Weapon
    {
        public EnemyWeapon(Game game) : base(game)
        {
        }

        public override void FireWeapon(Vector2 startPosition)
        {
        }

        public override void LoadShots()
        {
        }

        public override void UpdateShots()
        {
        }

        public override void DrawShots()
        {
        }

        public override IList<IShot> Shots
        {
            get
            {
                return new List<IShot>();
            }
        }
    }
}
