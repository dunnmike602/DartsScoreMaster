using Windows.Foundation;
using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Model
{
    public class Score : BaseViewModel
    {
        private int _value;
        private ScoreType _scoreType;
        private HiLo _hilo;
        private Point _position;
        private int _turnNumber;
        private int _numberHit;

        public int TurnNumber
        {
            get { return _turnNumber; }
            set
            {
                Set(() => TurnNumber, ref _turnNumber, value);
            }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                Set(() => Value, ref _value, value);
            }
        }

        public int NumberHit
        {
            get { return _numberHit; }
            set
            {
                Set(() => NumberHit, ref _numberHit, value);
            }
        }

        public HiLo HiLo
        {
            get { return _hilo; }
            set
            {
                Set(() => HiLo, ref _hilo, value);
            }
        }

        public ScoreType ScoreType
        {
            get { return _scoreType; }
            set
            {
                Set(() => ScoreType, ref _scoreType, value);
            }
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                Set(() => Position, ref _position, value);
            }
        }

        public Leg ScoredInLeg { get; set; }

        public bool IsSimpleBoard { get; set; }

        public int DartScore { get; set; }
        
        public bool GetCanCheckout()
        {
            return Value <= 170 && Value != 169 && Value != 168 && Value != 166 && Value != 165 && Value != 163 && Value != 162 && Value != 159;
        }

        public string DisplayText()
        {
            if (ScoreType == ScoreType.Double)
            {
                return Value + " (D" + (Value / 2) + ") ";
            }

            if (ScoreType == ScoreType.Treble)
            {
                return Value + " (T" + (Value / 3) + ") ";
            }

            if (ScoreType == ScoreType.Bull)
            {
                return Value + " (BULL) ";
            }
            
            if (ScoreType == ScoreType.OuterBull)
            {
                return Value + " (OBULL) ";
            }

            return Value + " ";
        }
    }
}