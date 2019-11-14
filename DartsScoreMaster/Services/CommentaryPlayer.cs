using System;
using System.Linq;
using DartsScoreMaster.Model;
using DartsScoreMaster.Services.Interfaces;
using Syncfusion.Data.Extensions;

namespace DartsScoreMaster.Services
{
    public class CommentaryPlayer : ICommentaryPlayer, IDisposable
    {
        private readonly IWavePlayer _wavePlayer;
   
        public CommentaryPlayer(IWavePlayer wavePlayer)
        {
            _wavePlayer = wavePlayer;
        }

        public void Play(Commentary commentary)
        {
            PlayHelper(commentary);
        }

        private void PlayHelper(Commentary commentary)
        {
            if (!commentary.PlaySounds)
            {
                return;
            }

            commentary.SoundFiles.Where(sound => !string.IsNullOrWhiteSpace(sound)).IterateIndex(
                (index, soundFile) => ProcessSoundItem(soundFile, commentary.GetSoundTextByIndex(index)));
        }

        private void ProcessSoundItem(string soundFile, string soundText)
        {
           _wavePlayer.PlayWave(soundFile, soundText);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _wavePlayer.Dispose();
            }
        }
    }
}