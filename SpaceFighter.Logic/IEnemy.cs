// -----------------------------------------------------------------------
// <copyright file="IEnemy.cs" company="Cataclysm">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IEnemy
    {
        Vector2 Position { get; }

        Texture2D ShipSprite { get; }
    }
}
