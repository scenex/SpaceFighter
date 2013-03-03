// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.AI
{
    using Microsoft.Xna.Framework;

    public class SteeringSeek : ISteering
    {
        const float Mass = 50;
        const float MaxVelocity = 6;
        const float MaxForce = 0.6f;
        const float MaxSpeed = 0.5f;

        Vector2 velocity;

        public Vector2 Run(Vector2 position, Vector2 distance, float angle)
        {
            var desiredVelocity = Vector2.Normalize(distance) * MaxVelocity;
            var steering = Vector2.Subtract(desiredVelocity, velocity);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            velocity = Vector2.Add(velocity, steering);
            velocity = velocity.Truncate(MaxSpeed);
            
            return position + this.velocity;
        }
    }
}
