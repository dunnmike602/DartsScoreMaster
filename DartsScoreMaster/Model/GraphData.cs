using GalaSoft.MvvmLight;

namespace DartsScoreMaster.Model
{
    public class GraphData : ObservableObject
    {
        private float _value;
        private string _name;
        private Statistic _statistic;

        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public float Value
        {
            get { return _value; }
            set
            {
                Set(() => Value, ref _value, value);
            }
        }

        public Statistic Statistic
        {
            get { return _statistic; }
            set
            {
                Set(() => Statistic, ref _statistic, value);
            }
        }
    }
}