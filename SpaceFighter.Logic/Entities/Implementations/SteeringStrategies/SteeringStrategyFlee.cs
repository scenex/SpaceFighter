// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Entities.Implementations.SteeringStrategies
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Entities.Interfaces;

    public class SteeringStrategyFlee : ISteeringStrategy
    {
        const float Mass = 50;
        const float MaxVelocity = 6;
        const float MaxForce = 0.6f;
        const float MaxSpeed = 0.5f;

        Vector2 velocity;

        public Vector2 Execute(Vector2 position, Vector2 distance)
        {
            var desiredVelocity = Vector2.Normalize(-distance) * MaxVelocity;
            var steering = Vector2.Subtract(desiredVelocity, this.velocity);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            this.velocity = Vector2.Add(this.velocity, steering);
            this.velocity = this.velocity.Truncate(MaxSpeed);

            return position + this.velocity;
        }
    }
}
