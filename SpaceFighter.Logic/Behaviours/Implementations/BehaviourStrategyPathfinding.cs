// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class BehaviourStrategyPathfinding : BehaviourStrategy
    {
        const float Mass = 50;
        const float MaxVelocity = 3;
        const float MaxForce = 0.6f;
        const float MaxSpeed = 0.5f;
        Vector2 velocity;

        public override Vector2 Execute(Vector2 source, Vector2 target)
        {
            // *** ARRIVAL ***
            var distance = new Vector2(target.X - source.X, target.Y - source.Y);
            var desiredVelocity = Vector2.Normalize(distance);

            if (distance.Length() <= 200)
            {
                desiredVelocity = Vector2.Multiply(desiredVelocity, MaxVelocity * distance.Length() / 200);
            }
            else
            {
                desiredVelocity = Vector2.Multiply(desiredVelocity, MaxVelocity);
            }

            var steering = Vector2.Subtract(desiredVelocity, this.velocity);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            this.velocity = Vector2.Add(this.velocity, steering);
            this.velocity = this.velocity.Truncate(MaxSpeed);

            return source + this.velocity;
            // ************
        }
    }
}
