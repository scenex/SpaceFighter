﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.AI
{
    using Microsoft.Xna.Framework;

    public interface ISteering
    {
        Vector2 Run(Vector2 position, Vector2 distance, float angle);
    }
}
