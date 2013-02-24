// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IWeapon
    {
        float Rotation { get; }
        Vector2 Position { get; }
        IList<IShot> Shots { get; }

        void SetRotation(float angle);
        void SetPosition(Vector2 pos);

        void FireWeapon(Vector2 startPosition, int offset, double angle);
    }
}