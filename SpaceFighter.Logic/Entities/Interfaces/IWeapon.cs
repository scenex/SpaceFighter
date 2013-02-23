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
        
        void LoadShots(string texturePath);
        void LoadTurret(string texturePath);
      
        void DrawShots();
        void DrawTurret();

        void UpdateShots();
        void UpdateTurret();

        IList<IShot> Shots { get; }
    }
}