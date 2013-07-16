// -----------------------------------------------------------------------
// (c) Cataclysm Game Studios 2012
// -----------------------------------------------------------------------

namespace SpaceFighter.Logic.Services.Implementations
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    using SpaceFighter.Logic.Services.Interfaces;

    public class AudioService : GameComponent, IAudioService
    {
        private readonly Game game;

        private AudioEngine audioEngine;
        private SoundBank soundBank;
        private WaveBank waveBank;

        public AudioService(Game game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();

            #if WINDOWS
                this.audioEngine = new AudioEngine(game.Content.RootDirectory + @"/Sounds/Win/SpaceFighter.Sound.xgs");
                this.waveBank = new WaveBank(this.audioEngine, game.Content.RootDirectory + @"/Sounds/Win/Wave Bank.xwb");
                this.soundBank = new SoundBank(this.audioEngine, game.Content.RootDirectory + @"/Sounds/Win/Sound Bank.xsb");
            #elif XBOX
                this.audioEngine = new AudioEngine(game.Content.RootDirectory + @"/Sounds/Xbox/SpaceFighter.Sound.xgs");
                this.waveBank = new WaveBank(this.audioEngine, game.Content.RootDirectory + @"/Sounds/Xbox/Wave Bank.xwb");
                this.soundBank = new SoundBank(this.audioEngine, game.Content.RootDirectory + @"/Sounds/Xbox/Sound Bank.xsb"); 
            #endif
        }

        public void PlaySound(string cue)
        {
            this.soundBank.GetCue(cue).Play();
        }
    }
}
