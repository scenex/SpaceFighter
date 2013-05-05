// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Tests
{
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    using Xunit;

    public class WorldServiceTests
    {
        private readonly IWorldService testee;

        public WorldServiceTests()
        {
            this.testee = new WorldService(null);
        }
    }
}
