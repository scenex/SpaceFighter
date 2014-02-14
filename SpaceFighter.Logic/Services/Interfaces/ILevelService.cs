// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ITerrainService : IGameComponent
    {
        int LevelWidth { get; }
        int LevelHeight { get; }

        int VerticalTileCount { get; }
        int HorizontalTileCount { get; }

        int TileSize { get; }
        int[,] Map { get; }
    }
}