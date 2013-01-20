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
        private Rectangle spriteRectangle;

        private string state;

        private Effect effect;

        public SpriteManager(string initialEntityState)
        {
            this.sprites = new Dictionary<string, Texture2D>();
            this.state = initialEntityState;
        }

        public bool IsAnimationDone
        {
            get
            {
                return this.currentFrame == FrameCount - 1;
            }
        }

        public void AddSprite(string entityState, Texture2D sprite) // <- associate shader to state / sprite? dictionary state / shader? associate actions for params in shader?
        {
            this.sprites.Add(entityState, sprite);
        }

        public void AddShader(Effect shader)
        {
            this.effect = shader;
        }

        public void Update(string entityState)
        {
            this.state = entityState;
        }

        public Texture2D GetCurrentSprite()
        {
            switch (state)
            {
                case PlayerState.Alive:
                    return this.sprites[PlayerState.Alive];

                case PlayerState.Dying:
                    return this.sprites[PlayerState.Dying];

                case PlayerState.Dead:
                    return this.sprites[PlayerState.Dead];

                case PlayerState.Respawn:
                    return this.sprites[PlayerState.Alive];

                default:
                    return null;
            }
        }

        public Rectangle GetCurrentRectangle(GameTime gameTime)
        {
            Rectangle currentRectangle;

            if (this.state == PlayerState.Dying)
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
            if (state == PlayerState.Respawn)
            {
                return this.effect;
            }

            return null;
        }

        public void SetRectangle(string playerState)
        {
            this.spriteRectangle = new Rectangle(0, 0, this.sprites[playerState].Width, this.sprites[playerState].Height);
        }

        public Rectangle GetRectangle()
        {
            return this.spriteRectangle;
        }
    }
}
