// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Behaviours.Implementations
{
    using Microsoft.Xna.Framework;
    using SpaceFighter.Logic.Services.Interfaces;

    public class BehaviourStrategySeek : BehaviourStrategy
    {
        const float Mass = 50;
        const float MaxVelocity = 3;
        const float MaxForce = 0.6f;
        const float MaxSpeed = 0.5f;
        Vector2 velocity;

        public BehaviourStrategySeek(IWorldService worldService) : base(worldService)
        {
        }

        public override Vector2 Execute(Vector2 source, Vector2 target)
        {
            var distance = new Vector2(target.X - source.X, target.Y - source.Y);
            var desiredVelocity = Vector2.Normalize(distance) * MaxVelocity;
            var steering = Vector2.Subtract(desiredVelocity, this.velocity);

            steering = steering.Truncate(MaxForce);
            steering = Vector2.Divide(steering, Mass);

            this.velocity = Vector2.Add(this.velocity, steering);
            this.velocity = this.velocity.Truncate(MaxSpeed);
            
            return source + this.velocity;
        }
    }
}
