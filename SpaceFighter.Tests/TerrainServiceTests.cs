// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Tests
{
    using SpaceFighter.Logic.Services.Implementations;
    using SpaceFighter.Logic.Services.Interfaces;

    using Xunit;

    public class TerrainServiceTests
    {
        private readonly ITerrainService testee;

        public TerrainServiceTests()
        {
            this.testee = new TerrainService(null);
        }
    }
}
