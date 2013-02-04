// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.Enemies
{   
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class EnemyGreen : EnemyBase
    {
        public EnemyGreen(Game game, IEnumerable<Vector2> waypoints) : base(game, waypoints)
        {
        }
    }
}
