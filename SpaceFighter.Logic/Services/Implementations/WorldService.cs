// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using System;
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class WorldService : GameComponent, IWorldService
    {
        public WorldService(Game game) : base(game)
        {
        }

        public void LoadWorld()
        {
            throw new NotImplementedException();
        }

        public void GetCollidableElements()
        {
            throw new NotImplementedException();
        }
    }
}