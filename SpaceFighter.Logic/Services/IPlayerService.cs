// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
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
