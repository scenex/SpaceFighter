// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IHeadUpDisplayService : IGameComponent
    {
        void Draw(GameTime gameTime, Color color);
        int Health { get; set; }
        int Lives { get; set; }
    }
}
