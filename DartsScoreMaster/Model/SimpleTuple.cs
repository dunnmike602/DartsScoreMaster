using GalaSoft.MvvmLight;

namespace DartsScoreMaster.Model
{
    public class SimpleTuple<TN,TV> : ObservableObject
    {
        private TV _value;
        private TN _name;

        public TN Name
        {
            get { return _name;}
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public TV Value
        {
            get { return _value; }
            set
            {
                Set(() => Value, ref _value, value);
            }
        }
    }
}