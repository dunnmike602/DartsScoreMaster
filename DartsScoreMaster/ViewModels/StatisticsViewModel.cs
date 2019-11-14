using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using DartsScoreMaster.Common;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using ReactiveUI;

namespace DartsScoreMaster.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IPlayersRepository _playersRepository;
        private string _nickName;
        private string _lastPlayed501;
        private string _lastPlayed401;
        private string _lastPlayed301;
        private string _lastPlayed201;
        private string _lastPlayed101;
        private string _lastPlayedCricket;
        private string _oneDart501;
        private string _oneDart401;
        private string _oneDart301;
        private string _oneDart201;
        private string _oneDart101;
        private string _oneDartCricket;
        private string _threeDart501;
        private string _threeDart401;
        private string _threeDart301;
        private string _threeDart201;
        private string _threeDart101;
        private string _threeDartCricket;
        private string _checkOutPc501;
        private string _checkOutPc401;
        private string _checkOutPc301;
        private string _checkOutPc201;
        private string _checkOutPc101;
        private string _checkOutPcCricket;
        private string _highThreeDartCricket;
        private string _highThreeDart101;
        private string _highThreeDart201;
        private string _highThreeDart301;
        private string _highThreeDart401;
        private string _highThreeDart501;
        private string _highCheckoutCricket;
        private string _highCheckout101;
        private string _highCheckout201;
        private string _highCheckout301;
        private string _highCheckout401;
        private string _highCheckout501;

        private string _dartsLeg501;
        private string _dartsLeg401;
        private string _dartsLeg301;
        private string _dartsLeg201;
        private string _dartsLeg101;

        private string _matchesWonPc501;
        private string _matchesWonPc401;
        private string _matchesWonPc301;
        private string _matchesWonPc201;
        private string _matchesWonPc101;
        private string _matchesWonPcCricket;
        private string _scoreOneEighty501;
        private string _scoreOneEighty401;
        private string _scoreOneEighty301;
        private string _scoreOneEighty201;
        private string _scoreOneEighty101;
        private string _scoreOneEightyCricket;
        private string _scoreOneForty501;
        private string _scoreOneForty401;
        private string _scoreOneForty301;
        private string _scoreOneForty201;
        private string _scoreOneForty101;
        private string _scoreOneFortyCricket;
        private string _scoreTon501;
        private string _scoreTon401;
        private string _scoreTon301;
        private string _scoreTon201;
        private string _scoreTon101;
        private string _scoreTonCricket;
        private string _nineDartCheckOut501;
        private string _nineDartCheckOut401;
        private string _nineDartCheckOut301;
        private string _nineDartCheckOut201;
        private string _nineDartCheckOut101;
        private string _nineDartCheckOutCricket;
        private string _twelveDartCheckOut501;
        private string _twelveDartCheckOut401;
        private string _twelveDartCheckOut301;
        private string _twelveDartCheckOut201;
        private string _twelveDartCheckOut101;
        private string _twelveDartCheckOutCricket;
        private string _bestGame501;
        private string _bestGame401;
        private string _bestGame301;
        private string _bestGame201;
        private string _bestGame101;
        private string _bestGameCricket;
        private string _chartTitle;
        private ReactiveList<SimpleTuple<string, decimal?>> _currentChartSeries;
        private Guid _playerId;
        private decimal? _interval;
        private decimal? _minimum;
        private decimal? _maximum;
        private GameType _currentGameType;
        private StatisticsSummary _playerStatistics;
        private int _periodValue;
        private bool _isChartVisible;
        private string _dartsLegCricket;

        public ReactiveCommand<object> ShowChartCommand { get; private set; }

        public ReactiveList<SimpleTuple<string, decimal?>> CurrentChartSeries
        {
            get { return _currentChartSeries; }
            set
            {
                Set(() => CurrentChartSeries, ref _currentChartSeries, value);
            }
        }


        public bool IsChartVisible
        {
            get { return _isChartVisible; }
            set
            {
                Set(() => IsChartVisible, ref _isChartVisible, value);
            }
        }


        public Guid PlayerId
        {
            get { return _playerId; }
            set
            {
                Set(() => PlayerId, ref _playerId, value);
            }
        }


        public int PeriodValue
        {
            get { return _periodValue; }
            set
            {
                Set(() => PeriodValue, ref _periodValue, value);
            }
        }

        public string NickName
        {
            get { return _nickName; }
            set
            {
                Set(() => NickName, ref _nickName, value);
            }
        }

        public string BestGameCricket
        {
            get { return _bestGameCricket; }
            set
            {
                Set(() => BestGameCricket, ref _bestGameCricket, value);
            }
        }

        public string BestGame101
        {
            get { return _bestGame101; }
            set
            {
                Set(() => BestGame101, ref _bestGame101, value);
            }
        }

        public string BestGame201
        {
            get { return _bestGame201; }
            set
            {
                Set(() => BestGame201, ref _bestGame201, value);
            }
        }

        public string BestGame301
        {
            get { return _bestGame301; }
            set
            {
                Set(() => BestGame301, ref _bestGame301, value);
            }
        }

        public string BestGame401
        {
            get { return _bestGame401; }
            set
            {
                Set(() => BestGame401, ref _bestGame401, value);
            }
        }

        public string BestGame501
        {
            get { return _bestGame501; }
            set
            {
                Set(() => BestGame501, ref _bestGame501, value);
            }
        }

        public string TwelveDartCheckOutCricket
        {
            get { return _twelveDartCheckOutCricket; }
            set
            {
                Set(() => TwelveDartCheckOutCricket, ref _twelveDartCheckOutCricket, value);
            }
        }

        public string TwelveDartCheckOut101
        {
            get { return _twelveDartCheckOut101; }
            set
            {
                Set(() => TwelveDartCheckOut101, ref _twelveDartCheckOut101, value);
            }
        }

        public string TwelveDartCheckOut201
        {
            get { return _twelveDartCheckOut201; }
            set
            {
                Set(() => TwelveDartCheckOut201, ref _twelveDartCheckOut201, value);
            }
        }

        public string TwelveDartCheckOut301
        {
            get { return _twelveDartCheckOut301; }
            set
            {
                Set(() => TwelveDartCheckOut301, ref _twelveDartCheckOut301, value);
            }
        }

        public string TwelveDartCheckOut401
        {
            get { return _twelveDartCheckOut401; }
            set
            {
                Set(() => TwelveDartCheckOut401, ref _twelveDartCheckOut401, value);
            }
        }

        public string TwelveDartCheckOut501
        {
            get { return _twelveDartCheckOut501; }
            set
            {
                Set(() => TwelveDartCheckOut501, ref _twelveDartCheckOut501, value);
            }
        }

        public string NineDartCheckOutCricket
        {
            get { return _nineDartCheckOutCricket; }
            set
            {
                Set(() => NineDartCheckOutCricket, ref _nineDartCheckOutCricket, value);
            }
        }

        public string NineDartCheckOut101
        {
            get { return _nineDartCheckOut101; }
            set
            {
                Set(() => NineDartCheckOut101, ref _nineDartCheckOut101, value);
            }
        }

        public string NineDartCheckOut201
        {
            get { return _nineDartCheckOut201; }
            set
            {
                Set(() => NineDartCheckOut201, ref _nineDartCheckOut201, value);
            }
        }

        public string NineDartCheckOut301
        {
            get { return _nineDartCheckOut301; }
            set
            {
                Set(() => NineDartCheckOut301, ref _nineDartCheckOut301, value);
            }
        }

        public string NineDartCheckOut401
        {
            get { return _nineDartCheckOut401; }
            set
            {
                Set(() => NineDartCheckOut401, ref _nineDartCheckOut401, value);
            }
        }

        public string NineDartCheckOut501
        {
            get { return _nineDartCheckOut501; }
            set
            {
                Set(() => NineDartCheckOut501, ref _nineDartCheckOut501, value);
            }
        }

        public string ScoreTonCricket
        {
            get { return _scoreTonCricket; }
            set
            {
                Set(() => ScoreTonCricket, ref _scoreTonCricket, value);
            }
        }

        public string ScoreTon101
        {
            get { return _scoreTon101; }
            set
            {
                Set(() => ScoreTon101, ref _scoreTon101, value);
            }
        }

        public string ScoreTon201
        {
            get { return _scoreTon201; }
            set
            {
                Set(() => ScoreTon201, ref _scoreTon201, value);
            }
        }

        public string ScoreTon301
        {
            get { return _scoreTon301; }
            set
            {
                Set(() => ScoreTon301, ref _scoreTon301, value);
            }
        }

        public string ScoreTon401
        {
            get { return _scoreTon401; }
            set
            {
                Set(() => ScoreTon401, ref _scoreTon401, value);
            }
        }

        public string ScoreTon501
        {
            get { return _scoreTon501; }
            set
            {
                Set(() => ScoreTon501, ref _scoreTon501, value);
            }
        }

        public string ScoreOneFortyCricket
        {
            get { return _scoreOneFortyCricket; }
            set
            {
                Set(() => ScoreOneFortyCricket, ref _scoreOneFortyCricket, value);
            }
        }

        public string ScoreOneForty101
        {
            get { return _scoreOneForty101; }
            set
            {
                Set(() => ScoreOneForty101, ref _scoreOneForty101, value);
            }
        }

        public string ScoreOneForty201
        {
            get { return _scoreOneForty201; }
            set
            {
                Set(() => ScoreOneForty201, ref _scoreOneForty201, value);
            }
        }

        public string ScoreOneForty301
        {
            get { return _scoreOneForty301; }
            set
            {
                Set(() => ScoreOneForty301, ref _scoreOneForty301, value);
            }
        }

        public string ScoreOneForty401
        {
            get { return _scoreOneForty401; }
            set
            {
                Set(() => ScoreOneForty401, ref _scoreOneForty401, value);
            }
        }

        public string ScoreOneForty501
        {
            get { return _scoreOneForty501; }
            set
            {
                Set(() => ScoreOneForty501, ref _scoreOneForty501, value);
            }
        }

        public string ScoreOneEightyCricket
        {
            get { return _scoreOneEightyCricket; }
            set
            {
                Set(() => ScoreOneEightyCricket, ref _scoreOneEightyCricket, value);
            }
        }

        public string ScoreOneEighty101
        {
            get { return _scoreOneEighty101; }
            set
            {
                Set(() => ScoreOneEighty101, ref _scoreOneEighty101, value);
            }
        }

        public string ScoreOneEighty201
        {
            get { return _scoreOneEighty201; }
            set
            {
                Set(() => ScoreOneEighty201, ref _scoreOneEighty201, value);
            }
        }

        public string ScoreOneEighty301
        {
            get { return _scoreOneEighty301; }
            set
            {
                Set(() => ScoreOneEighty301, ref _scoreOneEighty301, value);
            }
        }

        public string ScoreOneEighty401
        {
            get { return _scoreOneEighty401; }
            set
            {
                Set(() => ScoreOneEighty401, ref _scoreOneEighty401, value);
            }
        }

        public string ScoreOneEighty501
        {
            get { return _scoreOneEighty501; }
            set
            {
                Set(() => ScoreOneEighty501, ref _scoreOneEighty501, value);
            }
        }

        public string OneDart501
        {
            get { return _oneDart501; }
            set
            {
                Set(() => OneDart501, ref _oneDart501, value);
            }
        }

        public string OneDart401
        {
            get { return _oneDart401; }
            set
            {
                Set(() => OneDart401, ref _oneDart401, value);
            }
        }

        public string OneDart301
        {
            get { return _oneDart301; }
            set
            {
                Set(() => OneDart301, ref _oneDart301, value);
            }
        }

        public string OneDart201
        {
            get { return _oneDart201; }
            set
            {
                Set(() => OneDart201, ref _oneDart201, value);
            }
        }

        public string OneDart101
        {
            get { return _oneDart101; }
            set
            {
                Set(() => OneDart101, ref _oneDart101, value);
            }
        }

        public string OneDartCricket
        {
            get { return _oneDartCricket; }
            set
            {
                Set(() => OneDartCricket, ref _oneDartCricket, value);
            }
        }

        public string ThreeDart501
        {
            get { return _threeDart501; }
            set
            {
                Set(() => ThreeDart501, ref _threeDart501, value);
            }
        }

        public string ThreeDart401
        {
            get { return _threeDart401; }
            set
            {
                Set(() => ThreeDart401, ref _threeDart401, value);
            }
        }

        public string ThreeDart301
        {
            get { return _threeDart301; }
            set
            {
                Set(() => ThreeDart301, ref _threeDart301, value);
            }
        }

        public string ThreeDart201
        {
            get { return _threeDart201; }
            set
            {
                Set(() => ThreeDart201, ref _threeDart201, value);
            }
        }

        public string ThreeDart101
        {
            get { return _threeDart101; }
            set
            {
                Set(() => ThreeDart101, ref _threeDart101, value);
            }
        }

        public string ThreeDartCricket
        {
            get { return _threeDartCricket; }
            set
            {
                Set(() => ThreeDartCricket, ref _threeDartCricket, value);
            }
        }

        public string CheckOutPc501
        {
            get { return _checkOutPc501; }
            set
            {
                Set(() => CheckOutPc501, ref _checkOutPc501, value);
            }
        }

        public string CheckOutPc401
        {
            get { return _checkOutPc401; }
            set
            {
                Set(() => CheckOutPc401, ref _checkOutPc401, value);
            }
        }

        public string CheckOutPc301
        {
            get { return _checkOutPc301; }
            set
            {
                Set(() => CheckOutPc301, ref _checkOutPc301, value);
            }
        }

        public string CheckOutPc201
        {
            get { return _checkOutPc201; }
            set
            {
                Set(() => CheckOutPc201, ref _checkOutPc201, value);
            }
        }

        public string CheckOutPc101
        {
            get { return _checkOutPc101; }
            set
            {
                Set(() => CheckOutPc101, ref _checkOutPc101, value);
            }
        }

        public string CheckOutPcCricket
        {
            get { return _checkOutPcCricket; }
            set
            {
                Set(() => CheckOutPcCricket, ref _checkOutPcCricket, value);
            }
        }

        public string HighCheckoutCricket
        {
            get { return _highCheckoutCricket; }
            set
            {
                Set(() => HighCheckoutCricket, ref _highCheckoutCricket, value);
            }
        }

        public string HighCheckout101
        {
            get { return _highCheckout101; }
            set
            {
                Set(() => HighCheckout101, ref _highCheckout101, value);
            }
        }

        public string HighCheckout201
        {
            get { return _highCheckout201; }
            set
            {
                Set(() => HighCheckout201, ref _highCheckout201, value);
            }
        }

        public string HighCheckout301
        {
            get { return _highCheckout301; }
            set
            {
                Set(() => HighCheckout301, ref _highCheckout301, value);
            }
        }

        public string HighCheckout401
        {
            get { return _highCheckout401; }
            set
            {
                Set(() => HighCheckout401, ref _highCheckout401, value);
            }
        }

        public string HighCheckout501
        {
            get { return _highCheckout501; }
            set
            {
                Set(() => HighCheckout501, ref _highCheckout501, value);
            }
        }

        public string HighThreeDartCricket
        {
            get { return _highThreeDartCricket; }
            set
            {
                Set(() => HighThreeDartCricket, ref _highThreeDartCricket, value);
            }
        }

        public string HighThreeDart101
        {
            get { return _highThreeDart101; }
            set
            {
                Set(() => HighThreeDart101, ref _highThreeDart101, value);
            }
        }

        public string HighThreeDart201
        {
            get { return _highThreeDart201; }
            set
            {
                Set(() => HighThreeDart201, ref _highThreeDart201, value);
            }
        }


        public string HighThreeDart301
        {
            get { return _highThreeDart301; }
            set
            {
                Set(() => HighThreeDart301, ref _highThreeDart301, value);
            }
        }

        public string HighThreeDart401
        {
            get { return _highThreeDart401; }
            set
            {
                Set(() => HighThreeDart401, ref _highThreeDart401, value);
            }
        }

        public string HighThreeDart501
        {
            get { return _highThreeDart501; }
            set
            {
                Set(() => HighThreeDart501, ref _highThreeDart501, value);
            }
        }

        public string DartsLeg501
        {
            get { return _dartsLeg501; }
            set
            {
                Set(() => DartsLeg501, ref _dartsLeg501, value);
            }
        }

        public string DartsLeg401
        {
            get { return _dartsLeg401; }
            set
            {
                Set(() => DartsLeg401, ref _dartsLeg401, value);
            }
        }

        public string DartsLeg301
        {
            get { return _dartsLeg301; }
            set
            {
                Set(() => DartsLeg301, ref _dartsLeg301, value);
            }
        }

        public string DartsLeg201
        {
            get { return _dartsLeg201; }
            set
            {
                Set(() => DartsLeg201, ref _dartsLeg201, value);
            }
        }

        public string DartsLeg101
        {
            get { return _dartsLeg101; }
            set
            {
                Set(() => DartsLeg101, ref _dartsLeg101, value);
            }
        }


        public string MatchesWonPc501
        {
            get { return _matchesWonPc501; }
            set
            {
                Set(() => MatchesWonPc501, ref _matchesWonPc501, value);
            }
        }

        public string MatchesWonPc401
        {
            get { return _matchesWonPc401; }
            set
            {
                Set(() => MatchesWonPc401, ref _matchesWonPc401, value);
            }
        }

        public string MatchesWonPc301
        {
            get { return _matchesWonPc301; }
            set
            {
                Set(() => MatchesWonPc301, ref _matchesWonPc301, value);
            }
        }

        public string MatchesWonPc201
        {
            get { return _matchesWonPc201; }
            set
            {
                Set(() => MatchesWonPc201, ref _matchesWonPc201, value);
            }
        }

        public string MatchesWonPc101
        {
            get { return _matchesWonPc101; }
            set
            {
                Set(() => MatchesWonPc101, ref _matchesWonPc101, value);
            }
        }

        public string MatchesWonPcCricket
        {
            get { return _matchesWonPcCricket; }
            set
            {
                Set(() => MatchesWonPcCricket, ref _matchesWonPcCricket, value);
            }
        }

        public string DartsLegCricket
        {
            get { return _dartsLegCricket; }
            set
            {
                Set(() => DartsLegCricket, ref _dartsLegCricket, value);
            }
        }

        public string LastPlayed501
        {
            get { return _lastPlayed501; }
            set
            {
                Set(() => LastPlayed501, ref _lastPlayed501, value);
            }
        }

        public string LastPlayed401
        {
            get { return _lastPlayed401; }
            set
            {
                Set(() => LastPlayed401, ref _lastPlayed401, value);
            }
        }

        public string LastPlayed301
        {
            get { return _lastPlayed301; }
            set
            {
                Set(() => LastPlayed301, ref _lastPlayed301, value);
            }
        }

        public string LastPlayed201
        {
            get { return _lastPlayed201; }
            set
            {
                Set(() => LastPlayed201, ref _lastPlayed201, value);
            }
        }

        public string LastPlayed101
        {
            get { return _lastPlayed101; }
            set
            {
                Set(() => LastPlayed101, ref _lastPlayed101, value);
            }
        }

        public string LastPlayedCricket
        {
            get { return _lastPlayedCricket; }
            set
            {
                Set(() => LastPlayedCricket, ref _lastPlayedCricket, value);
            }
        }

        public String ChartTitle
        {
            get { return _chartTitle; }
            set
            {
                Set(() => ChartTitle, ref _chartTitle, value);
            }
        }

        public decimal? Interval
        {
            get { return _interval; }
            set
            {
                Set(() => Interval, ref _interval, value);
            }
        }

        public decimal? Minimum
        {
            get { return _minimum; }
            set
            {
                Set(() => Minimum, ref _minimum, value);
            }
        }

        public decimal? Maximum
        {
            get { return _maximum; }
            set
            {
                Set(() => Maximum, ref _maximum, value);
            }
        }

        public StatisticsViewModel(IStatisticsRepository statisticsRepository, IPlayersRepository playersRepository)
        {
            _statisticsRepository = statisticsRepository;
            _playersRepository = playersRepository;

            PeriodValue = 0;

            InitialiseSubscriptions();

            InitialiseCommands();

            SetUpMessageBus();
        }

        private void InitialiseCommands()
        {
            ShowChartCommand = ReactiveCommand.Create();
            ShowChartCommand.Subscribe(ShowChartCommandHandler);
        }

        private void ShowChartCommandHandler(object type)
        {
            var typedParameter = Convert.ToInt32(type);

            if (typedParameter == -1)
            {
                IsChartVisible = false;
            }
            else
            {
                _currentGameType = (GameType)(typedParameter);

                LoadPlayerPerformance();

                IsChartVisible = true;
            }
        }

        private void SetUpMessageBus()
        {
            MessageBus.Current.Listen<List<Player>>().Subscribe(CheckForChangedPlayer);
        }

        private void CheckForChangedPlayer(List<Player> players)
        {
            if (players.Any(m => m.PlayerDetails.Id == ParentUniqueKey))
            {
                LoadPlayer(ParentUniqueKey);
                LoadGraphData();
            }
        }

        private void InitialiseSubscriptions()
        {
            this.WhenAnyValue(x => x.ParentUniqueKey).Subscribe(LoadPlayer);

            this.WhenAnyValue(x => x.SelectedStatistic).Subscribe(_ => LoadGraphData());

            this.WhenAnyValue(x => x.PeriodValue).Subscribe(_ => LoadGraphData());
        }

        private void LoadPlayer(Guid playerId)
        {
            if (playerId == Guid.Empty)
            {
                return;
            }

            PlayerId = _playerId;
            _playerStatistics = _statisticsRepository.GetForPlayer(playerId);

            var player = _playersRepository.GetAll().FirstOrDefault(m => m.Id == playerId);

            LoadPlayerStats(player, _playerStatistics);

        }

        private void LoadPlayerPerformance()
        {
            ChartTitle = _currentGameType.GetAttribute<DisplayAttribute>().Name + ":";

            LoadGraphData();
        }

        private void LoadGraphData()
        {
            if (_playerStatistics == null)
            {
                return;
            }

            var statisticList = _playerStatistics.GameStatistics[_currentGameType];

            CurrentChartSeries = new ReactiveList<SimpleTuple<string, decimal?>>();

            switch (SelectedStatistic.Name)
            {
                case 0:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic == null ? (decimal?)null : Math.Round(stat.Statistic.OneDartAverage, 2))
                                })
                                .ToList());

                    Interval = 5;
                    Minimum = 0;
                    Maximum = 70;
                    break;

                case 1:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic == null ? (decimal?)null : Math.Round(stat.Statistic.ThreeDartAverage, 2))
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 190;
                    break;

                case 2:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic == null ? (decimal?)null : Math.Round(stat.Statistic.CheckoutPercentage, 2))
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 110;
                    break;

                case 3:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.HighestScore ?? 0)
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 190;
                    break;

                case 4:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.HighestCheckout ?? 0)
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 180;
                    break;

                case 5:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic == null ? 0 : Math.Round(stat.Statistic.LegsWonPc, 2))
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 110;
                    break;

                case 6:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic == null ? 0 : Math.Round(stat.Statistic.MatchesWonPc, 2))
                                })
                                .ToList());

                    Interval = 10;
                    Minimum = 0;
                    Maximum = 110;
                    break;

                case 7:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.HundredEightiesScored ?? 0)
                                })
                                .ToList());

                    decimal maxOneHundredEightiesValue = CurrentChartSeries.Max(m => m.Value) ?? 0;

                    Interval = Math.Round(maxOneHundredEightiesValue / 10, 0) + 1;
                    Minimum = 0;
                    Maximum = maxOneHundredEightiesValue + Interval;
                    break;

                case 8:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.HundredFortiesScored ?? 0)
                                })
                                .ToList());

                    decimal maxOneHundredFortiesValue = CurrentChartSeries.Max(m => m.Value) ?? 0;

                    Interval = Math.Round(maxOneHundredFortiesValue / 10, 0) + 1;
                    Minimum = 0;
                    Maximum = maxOneHundredFortiesValue + Interval;
                    break;



                case 9:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.HundredsScored ?? 0)
                                })
                                .ToList());

                    decimal maxOneHundredValue = CurrentChartSeries.Max(m => m.Value) ?? 0;

                    Interval = Math.Round(maxOneHundredValue / 10, 0) + 1;
                    Minimum = 0;
                    Maximum = maxOneHundredValue + Interval;
                    break;

                case 10:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.NineDartCheckouts ?? 0)
                                })
                                .ToList());

                    decimal maxNineDartValue = CurrentChartSeries.Max(m => m.Value) ?? 0;

                    Interval = Math.Round(maxNineDartValue / 10, 0) + 1;
                    Minimum = 0;
                    Maximum = maxNineDartValue + Interval;
                    break;

                case 11:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.TwelveDartCheckouts ?? 0)
                                })
                                .ToList());

                    decimal maxTwelveDartValue = CurrentChartSeries.Max(m => m.Value) ?? 0;

                    Interval = Math.Round(maxTwelveDartValue / 10, 0) + 1;
                    Minimum = 0;
                    Maximum = maxTwelveDartValue + Interval;
                    break;

                case 12:
                    CurrentChartSeries =
                        new ReactiveList<SimpleTuple<string, decimal?>>(
                            GetValuesByPeriod(statisticList, (Period)PeriodValue).Select(stat =>
                                new SimpleTuple<string, decimal?>
                                {
                                    Name = stat.Name,
                                    Value = (decimal?)(stat.Statistic?.BestGame ?? 0)
                                })
                                .ToList());

                    decimal maxBestGame = CurrentChartSeries.Max(m => m.Value) + 1 ?? 0;

                    Interval = Math.Round(maxBestGame / 10, 0);
                    Minimum = 0;
                    Maximum = maxBestGame + Interval;
                    break;
            }
        }

        private IEnumerable<GraphData> GetValuesByPeriod(IEnumerable<Statistic> statistics, Period period)
        {
            switch (period)
            {
                case Period.Monthly:
                    var months = GetMonths(12);
                    return months.GroupJoin(statistics, m => m.Item1 + "|" + m.Item2,
                        statistic => statistic.Date.Year + "|" + statistic.Date.Month,
                        (p, g) =>
                        {
                            var statisticsList = g as Statistic[] ?? g.ToArray();

                            return new GraphData
                            {
                                Name = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Item2) + "-" + p.Item1.ToString().Substring(2),
                                Statistic = !statisticsList.Any() ? null : statisticsList.Last()
                            };
                        }).ToList();

                case Period.Yearly:
                    var years = GetYears(10);

                    return years.GroupJoin(statistics, m => m,
                        statistic => statistic.Date.Year,
                        (p, g) =>
                        {
                            var statisticsList = g as Statistic[] ?? g.ToArray();
                            return new GraphData
                            {
                                Name = p.ToString(),
                                Statistic = !statisticsList.Any() ? null : statisticsList.Last()
                            };
                        }).ToList();

                case Period.Daily:
                    var days = GetDays(30);

                    var dailyStatistics = days.GroupJoin(statistics, m => m.Date,
                       statistic => statistic.Date.Date,
                       (p, g) =>
                       {
                           var statisticsList = g as Statistic[] ?? g.ToArray();

                           return new GraphData
                           {
                               Name = p.ToString("ddddd").Substring(0, 1).ToUpper(),
                               Statistic = !statisticsList.Any() ? null : statisticsList.Last()
                           };
                       }).ToList();

                    return dailyStatistics;

                default:
                    throw new ArgumentOutOfRangeException(nameof(period));
            }
        }

        private void LoadPlayerStats(PlayerDetails player, StatisticsSummary statistics)
        {
            if (player != null && statistics != null)
            {
                IsChartVisible = false;
                NickName = "LATEST PLAYER STATISTICS FOR " + player.NickName;
                LastPlayed501 = GetDate(statistics, GameType.T501);
                LastPlayed401 = GetDate(statistics, GameType.T401);
                LastPlayed301 = GetDate(statistics, GameType.T301);
                LastPlayed201 = GetDate(statistics, GameType.T201);
                LastPlayed101 = GetDate(statistics, GameType.T101);
                LastPlayedCricket = GetDate(statistics, GameType.Cricket);

                OneDart501 = GetIdAverage(statistics, GameType.T501);
                OneDart401 = GetIdAverage(statistics, GameType.T401);
                OneDart301 = GetIdAverage(statistics, GameType.T301);
                OneDart201 = GetIdAverage(statistics, GameType.T201);
                OneDart101 = GetIdAverage(statistics, GameType.T101);
                OneDartCricket = GetIdAverage(statistics, GameType.Cricket);

                ThreeDart501 = Get3DAverage(statistics, GameType.T501);
                ThreeDart401 = Get3DAverage(statistics, GameType.T401);
                ThreeDart301 = Get3DAverage(statistics, GameType.T301);
                ThreeDart201 = Get3DAverage(statistics, GameType.T201);
                ThreeDart101 = Get3DAverage(statistics, GameType.T101);
                ThreeDartCricket = Get3DAverage(statistics, GameType.Cricket);

                CheckOutPc501 = GetCheckOutPc(statistics, GameType.T501);
                CheckOutPc401 = GetCheckOutPc(statistics, GameType.T401);
                CheckOutPc301 = GetCheckOutPc(statistics, GameType.T301);
                CheckOutPc201 = GetCheckOutPc(statistics, GameType.T201);
                CheckOutPc101 = GetCheckOutPc(statistics, GameType.T101);
                CheckOutPcCricket = "N/A";

                HighThreeDart501 = GetHiThreeDarts(statistics, GameType.T501);
                HighThreeDart401 = GetHiThreeDarts(statistics, GameType.T401);
                HighThreeDart301 = GetHiThreeDarts(statistics, GameType.T301);
                HighThreeDart201 = GetHiThreeDarts(statistics, GameType.T201);
                HighThreeDart101 = GetHiThreeDarts(statistics, GameType.T101);
                HighThreeDartCricket = GetHiThreeDarts(statistics, GameType.Cricket);

                HighCheckout501 = GetHiCheckout(statistics, GameType.T501);
                HighCheckout401 = GetHiCheckout(statistics, GameType.T401);
                HighCheckout301 = GetHiCheckout(statistics, GameType.T301);
                HighCheckout201 = GetHiCheckout(statistics, GameType.T201);
                HighCheckout101 = GetHiCheckout(statistics, GameType.T101);
                HighCheckoutCricket = "N/A";

                DartsLeg501 = DartsPerLeg(statistics, GameType.T501);
                DartsLeg401 = DartsPerLeg(statistics, GameType.T401);
                DartsLeg301 = DartsPerLeg(statistics, GameType.T301);
                DartsLeg201 = DartsPerLeg(statistics, GameType.T201);
                DartsLeg101 = DartsPerLeg(statistics, GameType.T101);
                DartsLegCricket = "N/A";

                MatchesWonPc501 = MatchesWonPc(statistics, GameType.T501);
                MatchesWonPc401 = MatchesWonPc(statistics, GameType.T401);
                MatchesWonPc301 = MatchesWonPc(statistics, GameType.T301);
                MatchesWonPc201 = MatchesWonPc(statistics, GameType.T201);
                MatchesWonPc101 = MatchesWonPc(statistics, GameType.T101);
                MatchesWonPcCricket = MatchesWonPc(statistics, GameType.Cricket);

                ScoreOneEighty501 = Get180(statistics, GameType.T501);
                ScoreOneEighty401 = Get180(statistics, GameType.T401);
                ScoreOneEighty301 = Get180(statistics, GameType.T301);
                ScoreOneEighty201 = Get180(statistics, GameType.T201);
                ScoreOneEighty101 = "N/A";
                ScoreOneEightyCricket = Get180(statistics, GameType.Cricket);

                ScoreOneForty501 = Get140(statistics, GameType.T501);
                ScoreOneForty401 = Get140(statistics, GameType.T401);
                ScoreOneForty301 = Get140(statistics, GameType.T301);
                ScoreOneForty201 = Get140(statistics, GameType.T201);
                ScoreOneForty101 = "N/A";
                ScoreOneFortyCricket = Get140(statistics, GameType.Cricket);

                ScoreTon501 = Get100(statistics, GameType.T501);
                ScoreTon401 = Get100(statistics, GameType.T401);
                ScoreTon301 = Get100(statistics, GameType.T301);
                ScoreTon201 = Get100(statistics, GameType.T201);
                ScoreTon101 = "N/A";
                ScoreTonCricket = Get100(statistics, GameType.Cricket);

                NineDartCheckOut501 = Get9D(statistics, GameType.T501);
                NineDartCheckOut401 = Get9D(statistics, GameType.T401);
                NineDartCheckOut301 = Get9D(statistics, GameType.T301);
                NineDartCheckOut201 = Get9D(statistics, GameType.T201);
                NineDartCheckOut101 = Get9D(statistics, GameType.T201);
                NineDartCheckOutCricket = "N/A";

                TwelveDartCheckOut501 = Get12D(statistics, GameType.T501);
                TwelveDartCheckOut401 = Get12D(statistics, GameType.T401);
                TwelveDartCheckOut301 = Get12D(statistics, GameType.T301);
                TwelveDartCheckOut201 = Get12D(statistics, GameType.T201);
                TwelveDartCheckOut101 = Get12D(statistics, GameType.T201);
                TwelveDartCheckOutCricket = "N/A";

                BestGame501 = GetLowestDarts(statistics, GameType.T501);
                BestGame401 = GetLowestDarts(statistics, GameType.T401);
                BestGame301 = GetLowestDarts(statistics, GameType.T301);
                BestGame201 = GetLowestDarts(statistics, GameType.T201);
                BestGame101 = GetLowestDarts(statistics, GameType.T201);
                BestGameCricket = "N/A";
            }
        }

        private static string Get9D(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).NineDartCheckouts.ToString()
                : "-";
        }

        private static string Get12D(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).TwelveDartCheckouts.ToString()
                : "-";
        }

        private static string GetLowestDarts(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).BestGame.ToString()
                : "-";
        }

        private static string Get180(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).HundredEightiesScored.ToString()
                : "-";
        }

        private static string Get140(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).HundredFortiesScored.ToString()
                : "-";
        }

        private static string Get100(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).HundredsScored.ToString()
                : "-";
        }

        private static string GetHiCheckout(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).HighestCheckout.ToString()
                : "-";
        }

        private static string GetHiThreeDarts(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).HighestScore.ToString()
                : "-";
        }

        private static string GetIdAverage(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).OneDartAverage.ToString("####.00")
                : "-";
        }

        private static string MatchesWonPc(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).MatchesWonPc.ToString("###.00")
                : "-";
        }

        private static string DartsPerLeg(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0 && statistics.GetLatestStatisticForGame(type).LegsWon > 0
                ? (statistics.GetLatestStatisticForGame(type).DartsThrownInWinningLegs / (double)statistics.GetLatestStatisticForGame(type).LegsWon).ToString("###")
                : "-";
        }

        private static string GetCheckOutPc(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).CheckoutPercentage.ToString("###.00")
                : "-";
        }

        private static string Get3DAverage(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).ThreeDartAverage.ToString("####.00")
                : "-";
        }

        private static string GetDate(StatisticsSummary statistics, GameType type)
        {
            return statistics.GameStatistics.ContainsKey(type) && statistics.GameStatistics[type].Count > 0
                ? statistics.GetLatestStatisticForGame(type).LastPlayed
                : "-";
        }


    }
}