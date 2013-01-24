// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------


namespace SpaceFighter.Logic
{
    using System;
    using Microsoft.Xna.Framework.Graphics;

    public class Shader
    {
        private readonly string entityState;
        private readonly Effect effectAsset;
        private readonly Func<float, float> effectParameter;
        private readonly string effectParameterName;

        public Shader(string entityState, Effect effectAsset, Func<float, float> effectParameter, string effectParameterName)
        {
            this.entityState = entityState;
            this.effectAsset = effectAsset;
            this.effectParameter = effectParameter;
            this.effectParameterName = effectParameterName;
        }

        public string EntityState
        {
            get
            {
                return this.entityState;
            }
        }

        public Effect EffectAsset
        {
            get
            {
                return this.effectAsset;
            }
        }

        public Func<float, float> EffectParameter
        {
            get
            {
                return this.effectParameter;
            }
        }

        public string EffectParameterName
        {
            get
            {
                return this.effectParameterName;
            }
        }
    }
}
