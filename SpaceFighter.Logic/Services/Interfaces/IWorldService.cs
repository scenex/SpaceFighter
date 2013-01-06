// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    public interface IWorldService
    {
        int LevelWidth { get; }
        int LevelHeight { get; }

        void LoadWorld();
        void GetCollidableElements();
    }
}
