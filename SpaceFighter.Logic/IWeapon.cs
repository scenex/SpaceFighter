// -----------------------------------------------------------------------
// <copyright file="IWeapon.cs" company="Cataclysm">
// TODO: Update copyright text.
// </copyright>
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
