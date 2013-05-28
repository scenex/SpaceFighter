// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IPathFindingService
    {
        Queue<Vector2> GetPathToRandomTile(Vector2 sourcePosition);
    }
}