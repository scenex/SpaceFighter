// -----------------------------------------------------------------------
// <copyright file="EnemiesService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EnemiesService : GameComponent, IEnemiesServices
    {
        public EnemiesService(Game game) : base(game)
        {
        }

        public IEnumerable<IEnemy> Enemies
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
