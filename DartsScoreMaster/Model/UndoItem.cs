using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Model
{
    public class UndoItem : BaseViewModel
    {
        public int Score { get; set; }
        public int CurrentDartCount { get; set; }
        public int Hits20 { get; set; }
        public int Hits19 { get; set; }
        public int Hits18 { get; set; }
        public int Hits17 { get; set; }
        public int Hits16 { get; set; }
        public int Hits15 { get; set; }
        public int HitsBull { get; set; }
    }
}