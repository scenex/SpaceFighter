// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IWeapon
    {
        void FireWeapon(Vector2 startPosition, int offset, double angle);      
        IList<IShot> Shots { get; }
    }
}