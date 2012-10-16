// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IPlayerService
    {
        IPlayer Player { get; }
        void MoveLeft();
        void MoveRight();
        void MoveUp();
        void MoveDown();
        void Fire();
    }
}
