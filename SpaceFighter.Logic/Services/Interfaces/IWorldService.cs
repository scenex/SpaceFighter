// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IWorldService
    {
        int LevelWidth { get; }
        int LevelHeight { get; }
        int TileSize { get; }
        int[,] Map { get; }

        void LoadWorld();
        IEnumerable<int> GetCollidableTileIndices();
        IEnumerable<int> GetNonCollidableTileIndices();
    }
}
