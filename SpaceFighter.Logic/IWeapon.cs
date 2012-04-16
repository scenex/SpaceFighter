// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWeapon
    {
        void FireWeapon(Vector2 startPosition);

        Vector2[] ShotPositions { get; }
    }
}
