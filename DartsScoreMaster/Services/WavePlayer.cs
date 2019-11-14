using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using DartsScoreMaster.Services.Interfaces;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace DartsScoreMaster.Services
{
    public class WavePlayer : IWavePlayer 
    {
        private readonly SpeechSynthesizer _speechSynthesizer;
        private readonly XAudio2 _xAudio = new XAudio2();
        private SourceVoice _sourceVoice;
        private readonly AutoResetEvent _lock = new AutoResetEvent(true);

        public WavePlayer(SpeechSynthesizer speechSynthesizer)
        {
            _speechSynthesizer = speechSynthesizer;
            
            // ReSharper disable once ObjectCreationAsStatement
            new MasteringVoice(_xAudio);
        }

        private async Task<string> CreateAndGetFilePath(string soundText)
        {
            var stream = await _speechSynthesizer.SynthesizeTextToStreamAsync(soundText);

            var outputFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("TEMPCOM.WAV", CreationCollisionOption.ReplaceExisting);

            using (var reader = new DataReader(stream))
            {
                await reader.LoadAsync((uint)stream.Size);
                var buffer = reader.ReadBuffer((uint)stream.Size);
                await FileIO.WriteBufferAsync(outputFile, buffer);
            }

            return outputFile.Path;
        }

        private async Task<string> GetFilePath(string soundFile, string soundText)
        {
            var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets\\Commentary");

            string filePath;

            var file = await folder.TryGetItemAsync(soundFile) as StorageFile;

            if (file != null)
            {
                filePath = file.Path;
            }
            else
            {
                file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(soundFile) as StorageFile;

                filePath = file?.Path;
            }

            if (string.IsNullOrWhiteSpace(filePath) && !string.IsNullOrWhiteSpace(soundText))
            {
                filePath = await CreateAndGetFilePath(soundText);
            }

            return filePath;
        }

        public void PlayWave(string soundFile, string soundText)
        {
            Task.Run(() =>
            {
                _lock.WaitOne();
                PlayWaveHelper(soundFile, soundText);
            });
        }

        private void PlayWaveHelper(string soundFile, string soundText)
        {
            var filepath = GetFilePath(soundFile, soundText).Result;

            var nativefilestream = new NativeFileStream(filepath, NativeFileMode.Open, NativeFileAccess.Read);

            using (var soundstream = new SoundStream(nativefilestream))
            {
                var waveFormat = soundstream.Format;
                var buffer = new AudioBuffer
                {
                    Stream = soundstream.ToDataStream(),
                    AudioBytes = (int) soundstream.Length,
                    Flags = BufferFlags.EndOfStream
                };

                if (_sourceVoice != null)
                {
                    _sourceVoice.DestroyVoice();
                    _sourceVoice.Dispose();
                }

                _sourceVoice = new SourceVoice(_xAudio, waveFormat);

                _sourceVoice.SubmitSourceBuffer(buffer, soundstream.DecodedPacketsInfo);
                _sourceVoice.BufferEnd += obj =>
                {
                    _lock.Set();
                };
                
                _sourceVoice.Start();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _xAudio.Dispose();
            }
        }
    }
}