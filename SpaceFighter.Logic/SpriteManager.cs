// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteManager
    {
        private float totalElapsedTimeAnimation;

        private float totalElapsedTimeShader;

        private int currentFrame;
        private const float TimePerFrame = 0.0166667f * 50;
        
        private readonly Dictionary<string, Texture2D> sprites;
        private readonly Dictionary<string, Effect> effects;
        private readonly Dictionary<string, bool> animations;

        private readonly Rectangle spriteRectangle;

        private string state;

        private readonly int spriteHeight;

        private readonly int spriteWidth;

        private GameTime gameTime;

        private Func<float, float> effectParam;

        public SpriteManager(string initialEntityState, int spriteHeight, int spriteWidth)
        {
            this.sprites = new Dictionary<string, Texture2D>();
            this.effects = new Dictionary<string, Effect>();
            this.animations = new Dictionary<string, bool>();

            this.state = initialEntityState;
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;

            this.spriteRectangle = new Rectangle(0, 0, this.spriteWidth, this.spriteHeight);     
        }

        public bool IsAnimationDone(string entityState)
        {
            return this.currentFrame == (this.sprites[entityState].Width / this.spriteWidth) - 1;          
        }

        public void AddStillSprite(string entityState, Texture2D stillSprite)
        {
            this.sprites.Add(entityState, stillSprite);
            this.animations.Add(entityState, false);
        }

        public void AddStillSprite(string entityState, Texture2D stillSprite, Effect shader, Func<float, float> shaderParam) // <- 'Effect, ShaderParamName[], ShaderParamFunc[]' -> into some structure.
        {
            this.sprites.Add(entityState, stillSprite);
            this.effects.Add(entityState, shader);
            this.animations.Add(entityState, false);
            this.effectParam = shaderParam;
        }

        public void AddAnimatedSprite(string entityState, Texture2D animatedSprite)
        {
            this.sprites.Add(entityState, animatedSprite);
            this.animations.Add(entityState, true);
        }

        public void Update(string entityState, GameTime gameTime)
        {
            this.state = entityState;
            this.gameTime = gameTime;
        }

        public Texture2D GetCurrentSprite()
        {
            return this.sprites[this.state];
        }

        public Rectangle GetCurrentRectangle()
        {
            Rectangle currentRectangle;

            if (this.animations[this.state])
            {
                currentRectangle = new Rectangle(this.currentFrame * this.spriteWidth, 0, this.spriteWidth, this.spriteHeight);
                this.totalElapsedTimeAnimation += (float)this.gameTime.ElapsedGameTime.TotalSeconds;

                if (this.currentFrame != this.sprites[this.state].Width / this.spriteWidth - 1)
                {
                    if (this.totalElapsedTimeAnimation > TimePerFrame)
                    {
                        this.currentFrame++;
                        this.totalElapsedTimeAnimation = 0;
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
                this.totalElapsedTimeShader += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.effects[this.state].Parameters["param1"].SetValue(this.effectParam.Invoke(this.totalElapsedTimeShader));
                return this.effects[this.state];
            }

            this.totalElapsedTimeShader = 0;
            return null;
        }
    }
}
