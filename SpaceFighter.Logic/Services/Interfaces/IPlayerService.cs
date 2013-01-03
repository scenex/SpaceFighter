// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Interfaces
{
    using System.Collections.Generic;
    using SpaceFighter.Logic.Entities.Interfaces;

    public interface IPlayerService
    {
        IPlayer Player { get; }
        void MoveLeft();
        void MoveRight();
        void MoveUp();
        void MoveDown();
        void RotateLeft();
        void RotateRight();    
        void Fire();
        IEnumerable<IShot> Shots { get; } 
        void ReportPlayerHit(IShot shot);
        void RemoveShot(IShot shot);
    }
}
