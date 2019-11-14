using GalaSoft.MvvmLight;

namespace DartsScoreMaster.Model
{
    public class Game : ObservableObject
    {
        private string _name;
        private int _startingScore;
        private GameType _id;

        public GameType Id
        {
            get { return _id; }
            set
            {
                Set(() => Id, ref _id, value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public string PlayerTemplateName { get; set; }

        public string ClassId { get; set; }

        public bool ShowCheckOutHints { get; set; }

        public int StartingScore
        {
            get { return _startingScore; }
            set
            {
                Set(() => StartingScore, ref _startingScore, value);
            }
        }

        public int GetStartingScore(PlayerDetails currentPlayerDetails)
        {
            if (currentPlayerDetails == null)
            {
                return StartingScore;
            }

            if (Id == GameType.T501)
            {
                return StartingScore - currentPlayerDetails.Handicap501;
            }

            if (Id == GameType.T401)
            {
                return StartingScore - currentPlayerDetails.Handicap401;
            }

            if (Id == GameType.T301)
            {
                return StartingScore - currentPlayerDetails.Handicap301;
            }

            if (Id == GameType.T201)
            {
                return StartingScore - currentPlayerDetails.Handicap201;
            }

            if (Id == GameType.T101)
            {
                return StartingScore - currentPlayerDetails.Handicap101;
            }

            if (Id == GameType.Cricket)
            {
                return StartingScore + currentPlayerDetails.HandicapCricket;
            }

            return StartingScore;
        }
    }
}
