using System;
using DartsScoreMaster.Common;
using DartsScoreMaster.ViewModels;

namespace DartsScoreMaster.Model
{
    public class PlayerDetails : BaseViewModel
    {
        private ImageDefinition _playerImageDefinition;
        private string _name;
        private string _nickName;
        private int _handicap501;
        private int _handicap401;
        private int _handicap301;
        private int _handicap201;
        private int _handicap101;
        private int _handicapCricket;
        private Flight _selectedFlight;
        private StatisticsSummary _statistics = new StatisticsSummary();
        private Statistic _statistic = new Statistic();


        public Guid Id { get; set; }


        public Flight SelectedFlight
        {
            get
            {
                return _selectedFlight;
            }

            set
            {
                if (value != _selectedFlight)
                {
                    IsDirty = true;
                }

                Set(() => SelectedFlight, ref _selectedFlight, value);
            }
        }


        public ImageDefinition PlayerImageDefinition
        {
            get { return _playerImageDefinition; }
            set
            {
                if (value != _playerImageDefinition)
                {
                    IsDirty = true;
                }

                Set(() => PlayerImageDefinition, ref _playerImageDefinition, value);
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

        public StatisticsSummary Statistics
        {
            get { return _statistics; }
            set
            {
                Set(() => Statistics, ref _statistics, value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    IsDirty = true;
                }

                Set(() => Name, ref _name, value);
            }
        }

        public string NickName
        {
            get { return _nickName; }
            set
            {
                if (value != _nickName)
                {
                    IsDirty = true;
                }

                Set(() => NickName, ref _nickName, value);
            }
        }


        public int Handicap501
        {
            get { return _handicap501; }
            set
            {
                if (value != _handicap501)
                {
                    IsDirty = true;
                }

                Set(() => Handicap501, ref _handicap501, value);
            }
        }


        public int Handicap401
        {
            get { return _handicap401; }
            set
            {
                if (value != _handicap401)
                {
                    IsDirty = true;
                }

                Set(() => Handicap401, ref _handicap401, value);
            }
        }


        public int Handicap301
        {
            get { return _handicap301; }
            set
            {
                if (value != _handicap301)
                {
                    IsDirty = true;
                }

                Set(() => Handicap301, ref _handicap301, value);
            }
        }


        public int Handicap201
        {
            get { return _handicap201; }
            set
            {
                if (value != _handicap201)
                {
                    IsDirty = true;
                }

                Set(() => Handicap201, ref _handicap201, value);
            }
        }


        public int Handicap101
        {
            get { return _handicap101; }
            set
            {
                if (value != _handicap101)
                {
                    IsDirty = true;
                }

                Set(() => Handicap101, ref _handicap101, value);
            }
        }


        public int HandicapCricket
        {
            get { return _handicapCricket; }
            set
            {
                if (value != _handicapCricket)
                {
                    IsDirty = true;
                }

                Set(() => HandicapCricket, ref _handicapCricket, value);
            }
        }

        public async void RegenerateImage()
        {
            if (PlayerImageDefinition == null)
            {
                PlayerImageDefinition = new ImageDefinition();
            }

            if (PlayerImageDefinition != null)
            {
                PlayerImageDefinition.Source = await PlayerImageDefinition.SourceBytes.AsBitmapImageAsync();
            }

            SelectedFlight.Image = StandingData.GetFlights()[SelectedFlight.Index - 1].Image;
        }

        public PlayerDetails Clone()
        {
            var newObject = new PlayerDetails
            {
                Id = Id,
                Handicap101 = Handicap101,
                Handicap201 = Handicap201,
                Handicap301 = Handicap301,
                Handicap401 = Handicap401,
                Handicap501 = Handicap501,
                HandicapCricket = HandicapCricket,
                Name = Name,
                NickName = NickName,
                IsDirty = false,
                PlayerImageDefinition = new ImageDefinition()
            };

            if (PlayerImageDefinition != null)
            {
                newObject.PlayerImageDefinition.SourceBytes = PlayerImageDefinition.SourceBytes;
            }

            newObject.SelectedFlight = StandingData.GetFlights()[SelectedFlight.Index - 1];

            newObject.RegenerateImage();

            return newObject;
        }

        public void CopyFrom(PlayerDetails playerDetails)
        {
            Handicap101 = playerDetails.Handicap101;
            Handicap201 = playerDetails.Handicap201;
            Handicap301 = playerDetails.Handicap301;
            Handicap401 = playerDetails.Handicap401;
            Handicap501 = playerDetails.Handicap501;
            HandicapCricket = playerDetails.HandicapCricket;
            Name = playerDetails.Name;
            NickName = playerDetails.NickName;

            if (PlayerImageDefinition != null && playerDetails.PlayerImageDefinition != null)
            {
                PlayerImageDefinition.SourceBytes = playerDetails.PlayerImageDefinition.SourceBytes;
            }
            else
            {
                PlayerImageDefinition = new ImageDefinition();
            }

            SelectedFlight = StandingData.GetFlights()[playerDetails.SelectedFlight.Index - 1];

            RegenerateImage();
        }
    }
}