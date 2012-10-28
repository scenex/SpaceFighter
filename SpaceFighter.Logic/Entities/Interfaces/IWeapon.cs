// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public interface IWeapon
    {
        void FireWeapon(Vector2 startPosition);
        void LoadShots();
        void UpdateShots();
        void DrawShots();
        IList<IShot> Shots { get; }           
    }
}