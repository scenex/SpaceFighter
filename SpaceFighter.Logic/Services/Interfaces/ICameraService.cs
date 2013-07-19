// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface ICameraService : IGameComponent
    {
        Vector2 Position { get; set; }
        float Zoom { get; set; }
        float Rotation { get; set; }
        void Move(Vector2 amount);
        Matrix GetTransformation();
    }
}
