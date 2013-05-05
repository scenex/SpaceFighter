﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class BehaviourStrategyPathfinding : BehaviourStrategy
    {
        const float Mass = 50;
        const float MaxVelocity = 6;
        const float MaxForce = 0.6f;
        const float MaxSpeed = 0.5f;
        Vector2 velocity;

        private readonly int[,] map;
        private readonly int tileSize;

        private int sourceTile;
        private int destinationTile;

        public BehaviourStrategyPathfinding(IWorldService worldService) : base(worldService)
        {
            this.map = this.WorldService.Map;
            this.tileSize = this.WorldService.TileSize;
        }

        public override Vector2 Execute(Vector2 source, Vector2 target)
        {
            /* Ignore target parameter here, 
             * target is a non-collidable random tile in case of the pathfinding algorithm.
             * Every time a target has been reached, a new target is generated
             */

            this.sourceTile = ((int)source.X / tileSize) + ((int)source.Y / tileSize) * (this.map.GetUpperBound(1) + 1);
            this.destinationTile = ((int)target.X / tileSize) + ((int)target.Y / tileSize) * (this.map.GetUpperBound(1) + 1);

            // *** SEEK ***
            var distance = new Vector2(target.X - source.X, target.Y - source.Y);
            var desiredVelocity = Vector2.Normalize(distance) * MaxVelocity;
            var steering = Vector2.Subtract(desiredVelocity, this.velocity);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            this.velocity = Vector2.Add(this.velocity, steering);
            this.velocity = this.velocity.Truncate(MaxSpeed);

            return source + this.velocity;
            // ************

            //return source;
        }
    }
}