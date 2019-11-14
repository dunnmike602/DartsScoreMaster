using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace DartsScoreMaster.Model
{
    public class HintList : ObservableObject
    {
        private List<Hint> _hintDarts;

        public List<Hint> HintDarts
        {
            get { return _hintDarts; }
            set
            {
                Set(() => HintDarts, ref _hintDarts, value);
            }
        }
    }
}