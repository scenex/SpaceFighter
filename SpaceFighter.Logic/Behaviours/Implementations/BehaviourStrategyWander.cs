﻿// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using System;
    using Microsoft.Xna.Framework;

    public class BehaviourStrategyWander : BehaviourStrategy
    {
        const float Mass = 2;
        const float MaxVelocity = 3;
        const float MaxForce = 0.6f;

        const int CircleDistance = 6;
        const int CircleRadius = 8;
        const double AngleChange = 1.0f;

        double wanderAngle = 0;

        readonly Random random = new Random();

        Vector2 velocity = new Vector2(-1,-2);

        public override Vector2 Execute(Vector2 source, Vector2 target)
        {
            //return source;

            // Todo: Optimize, remove jitter and avoid walls.
            var circleCenter = velocity;
            circleCenter.Normalize();
            circleCenter = Vector2.Multiply(circleCenter, CircleDistance);

            var displacement = new Vector2(0, -1);
            displacement = Vector2.Multiply(displacement, CircleRadius);

            displacement.X = (float)Math.Cos(wanderAngle) * displacement.Length();
            displacement.Y = (float)Math.Sin(wanderAngle) * displacement.Length();

            wanderAngle += random.Next(0, 359) * AngleChange * 5;

            // steering aka wanderForce
            var steering = Vector2.Add(circleCenter, displacement);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            this.velocity = Vector2.Add(this.velocity, steering);
            this.velocity = this.velocity.Truncate(MaxVelocity);

            return source + this.velocity;
        }
    }
}
