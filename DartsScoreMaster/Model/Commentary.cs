namespace DartsScoreMaster.Model
{
    public class Commentary
    {
        public string[] SoundFiles { get; set; } = new string[0];

        public string[] SoundTexts { get; set; } = new string[0];

        public bool PlaySounds { get; set; }

        public string GetSoundTextByIndex(int index)
        {
            return SoundTexts.Length >= index + 1 ? SoundTexts[index] : null;
        }
    }
}