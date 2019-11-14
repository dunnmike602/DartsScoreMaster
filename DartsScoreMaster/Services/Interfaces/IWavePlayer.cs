using System;

namespace DartsScoreMaster.Services.Interfaces
{
    public interface IWavePlayer : IDisposable
    {
        void PlayWave(string soundFile, string soundText);
    }
}