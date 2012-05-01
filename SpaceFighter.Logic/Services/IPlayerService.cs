// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPlayerService
    {
        IPlayer Player { get; }

        void MoveLeft();

        void MoveRight();

        void MoveUp();

        void MoveDown();

        void UpgradeWeapon();

        void DowngradeWeapon();

        void FireWeapon();
    }
}
