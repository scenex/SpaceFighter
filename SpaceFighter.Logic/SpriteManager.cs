// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteManager
    {
        private float totalElapsed;

        private int currentFrame;
        private const int FrameCount = 16;
        private const float TimePerFrame = 0.0166667f * 3;
        
        private readonly Dictionary<string, Texture2D> sprites;
        private readonly Dictionary<string, Effect> effects;
        private readonly Dictionary<string, bool> animations;

        private Rectangle spriteRectangle;

        private string state;

        public SpriteManager(string initialEntityState)
        {
            this.sprites = new Dictionary<string, Texture2D>();
            this.effects = new Dictionary<string, Effect>();
            this.animations = new Dictionary<string, bool>();

            this.state = initialEntityState;
        }

        public bool IsAnimationDone
        {
            get
            {
                return this.currentFrame == FrameCount - 1;
            }
        }

        public void AddSprite(string entityState, Texture2D sprite, bool isAnimated)
        {
            this.sprites.Add(entityState, sprite);
            this.animations.Add(entityState, isAnimated);
        }

        public void AddSprite(string entityState, Texture2D sprite, Effect shader, bool isAnimated)
        {
            this.sprites.Add(entityState, sprite);
            this.effects.Add(entityState, shader);
            this.animations.Add(entityState, isAnimated);
        }

        public void Update(string entityState)
        {
            this.state = entityState;
        }

        public Texture2D GetCurrentSprite()
        {
            return this.sprites[this.state];
        }

        public Rectangle GetCurrentRectangle(GameTime gameTime) // <- hide rectangle handling from consumer
        {
            Rectangle currentRectangle;

            if (this.animations[this.state])
            {
                currentRectangle = new Rectangle(this.currentFrame * this.spriteRectangle.Width, 0, this.spriteRectangle.Width, this.spriteRectangle.Height);
                this.totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.currentFrame != FrameCount - 1)
                {
                    if (this.totalElapsed > TimePerFrame)
                    {
                        this.currentFrame++;
                        this.currentFrame = this.currentFrame % FrameCount;
                        this.totalElapsed = 0;
                    }
                }
            }
            else
            {
                currentRectangle = this.spriteRectangle;
                this.currentFrame = 0;
            }

            return currentRectangle;
        }

        public Effect GetCurrentShader()
        {
            if (this.effects.ContainsKey(this.state))
            {
                return this.effects[this.state];
            }

            return null;
        }

        public void SetRectangle(string entityState)
        {
            this.spriteRectangle = new Rectangle(0, 0, this.sprites[entityState].Width, this.sprites[entityState].Height);
        }

        public Rectangle GetRectangle()
        {
            return this.spriteRectangle;
        }
    }
}
