// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Interfaces
{
    public interface ISpaceFighterApi
    {
        void Load();
        void Save();
        void Pause();
        void Restart();
        void Quit();

        void AddEnemy();
        void RemoveEnemy();
        void ListEnemies();
    }
}
