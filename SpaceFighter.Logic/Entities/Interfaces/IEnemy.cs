// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IEnemy : IEntity
    {
        void SubtractHealth(int amount);
        void AddHealth(int amount);

        // Todo: Move to separate service?
        void UpdatePlayerPosition(Vector2 position);
    }
}
