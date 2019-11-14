using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.Controls.Interfaces;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Services.Interfaces;
using DartsScoreMaster.ViewModels.Interfaces;
using ReactiveUI;
using Syncfusion.Data.Extensions;

namespace DartsScoreMaster.ViewModels
{
    public class MainPageViewModel : VoiceControlViewModel, INavigable
    {
        private bool _isUndoEnabled;
        private string _gameButtonCaption;
        private bool _isSoundRecognised;
        private string _soundText;
        private bool _showSimpleBoard;
        private Player _inPlayPlayer;
        private bool _isAcceptOpen;
        private int _nextUpIndex;
        private readonly IPlayersRepository _playersRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly Func<string, IDartGame> _dartGameFactory;
        private readonly Func<string, IStatisticsCalculationService> _statisticsFactory;
        private int _selectedPlayers;
        private ReactiveList<Player> _participatingPlayers = new ReactiveList<Player>();
        private bool _isGameRunning;
        private Score _dartScore;
        private int _dartTotal;
        private bool _enabled;
        private int _currentPlayer;
        private List<HintList> _hint;
        private int _selectedLeg;
        private int _selectedSet;
        private Guid _currentGameId;
        private int _currentTurn;
        private string _matchDetails;
        private bool _matchWon;
        private string _dartImageName;
        private BitmapImage _selectedPlayerImage;
        private bool _isFlyoutClosed;
        private string _throw1;
        private string _throw2;
        private string _throw3;
        private string _totalThrow;
        private ReactiveList<PlayerDetails> _playersList;
        private ObservableCollection<Player> _configuredPlayers = new ObservableCollection<Player>();
        private GameConfiguration _configurationData;
        private List<Tuple<int, string>> _checkoutHints;
        private IDartGame _currentGameService;
        private IStatisticsCalculationService _currentStatisticsService;
        private DataTemplate _playerTemplate;
        private bool _playSound;
        private BitmapImage _flipImageSource;
        private bool _isBoardEnabled;
      
        public GameConfiguration ConfigurationData
        {
            get { return _configurationData; }
            set { Set(() => ConfigurationData, ref _configurationData, value); }
        }

        public bool IsAcceptOpen
        {
            get { return _isAcceptOpen; }
            set { Set(() => IsAcceptOpen, ref _isAcceptOpen, value); }
        }

        public bool IsFlyoutClosed
        {
            get { return _isFlyoutClosed; }
            set { Set(() => IsFlyoutClosed, ref _isFlyoutClosed, value); }
        }

        public bool MatchWon
        {
            get { return _matchWon; }
            set { Set(() => MatchWon, ref _matchWon, value); }
        }

        public List<HintList> HintList
        {
            get { return _hint; }
            set { Set(() => HintList, ref _hint, value); }
        }

        public string GameButtonCaption
        {
            get { return _gameButtonCaption; }
            set { Set(() => GameButtonCaption, ref _gameButtonCaption, value); }
        }

        public bool IsUndoEnabled
        {
            get { return _isUndoEnabled; }
            set { Set(() => IsUndoEnabled, ref _isUndoEnabled, value); }
        }

        public bool IsGameRunning
        {
            get { return _isGameRunning; }
            set
            {
                Set(() => IsGameRunning, ref _isGameRunning, value);
                RaisePropertyChanged();
            }
        }

        public Player InPlayPlayer
        {
            get { return _inPlayPlayer; }
            set { Set(() => InPlayPlayer, ref _inPlayPlayer, value); }
        }

        public ReactiveList<Player> ParticipatingPlayers
        {
            get { return _participatingPlayers; }
            set { Set(() => ParticipatingPlayers, ref _participatingPlayers, value); }
        }

        public List<Tuple<int, string>> CheckoutHints
        {
            get { return _checkoutHints; }
            set { Set(() => CheckoutHints, ref _checkoutHints, value); }
        }

        public int CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                Set(() => CurrentPlayer, ref _currentPlayer, value);
                InPlayPlayer = ParticipatingPlayers[CurrentPlayer];
            }
        }

        public int SelectedPlayers
        {
            get { return _selectedPlayers; }
            set { Set(() => SelectedPlayers, ref _selectedPlayers, value); }
        }

        public int SelectedLeg
        {
            get { return _selectedLeg; }
            set { Set(() => SelectedLeg, ref _selectedLeg, value); }
        }

        public int SelectedSet
        {
            get { return _selectedSet; }
            set { Set(() => SelectedSet, ref _selectedSet, value); }
        }
        
        public ReactiveCommand<object> StartGameCommand { get; private set; }
        public ReactiveCommand<object> ResetCommand { get; private set; }
        public ReactiveCommand<object> OpenNewGameWindow { get; private set; }
        public ReactiveCommand<object> AcceptScoreCommand { get; private set; }
        public ReactiveCommand<object> RejectScoreCommand { get; private set; }
        public ReactiveCommand<object> FlipCommand { get; private set; }
        public ReactiveCommand<object> UndoCommand { get; private set; }
        public ReactiveCommand<object> StopGameCommand { get; private set; }

        public Score DartScore
        {
            get { return _dartScore; }
            set { Set(() => DartScore, ref _dartScore, value); }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { Set(() => Enabled, ref _enabled, value); }
        }

        public ReactiveList<PlayerDetails> PlayersList
        {
            get { return _playersList; }
            set { Set(() => PlayersList, ref _playersList, value); }
        }

        public ObservableCollection<Player> ConfiguredPlayers
        {
            get { return _configuredPlayers; }
            set { Set(() => ConfiguredPlayers, ref _configuredPlayers, value); }
        }

        public string TotalThrow
        {
            get { return _totalThrow; }
            set { Set(() => TotalThrow, ref _totalThrow, value); }
        }

        public string Throw1
        {
            get { return _throw1; }
            set { Set(() => Throw1, ref _throw1, value); }
        }

        public string Throw2
        {
            get { return _throw2; }
            set { Set(() => Throw2, ref _throw2, value); }
        }

        public string Throw3
        {
            get { return _throw3; }
            set { Set(() => Throw3, ref _throw3, value); }
        }

        public string MatchDetails
        {
            get { return _matchDetails; }
            set { Set(() => MatchDetails, ref _matchDetails, value); }
        }

        public BitmapImage SelectedPlayerImage
        {
            get { return _selectedPlayerImage; }
            set { Set(() => SelectedPlayerImage, ref _selectedPlayerImage, value); }
        }
        
        public bool ShowSimpleBoard
        {
            get { return _showSimpleBoard; }
            set { Set(() => ShowSimpleBoard, ref _showSimpleBoard, value); }
        }

        public string DartImageName
        {
            get { return _dartImageName; }
            set { Set(() => DartImageName, ref _dartImageName, value); }
        }

        public DataTemplate PlayerTemplate
        {
            get { return _playerTemplate; }
            set { Set(() => PlayerTemplate, ref _playerTemplate, value); }
        }

        public bool PlaySound
        {
            get { return _playSound; }
            set { Set(() => PlaySound, ref _playSound, value); }
        }

        public BitmapImage FlipImageSource
        {
            get { return _flipImageSource; }
            set { Set(() => FlipImageSource, ref _flipImageSource, value); }
        }

        public bool IsBoardEnabled
        {
            get { return _isBoardEnabled && IsGameRunning; }
            set
            {
                Set(() => IsBoardEnabled, ref _isBoardEnabled, value);
                RaisePropertyChanged();
            }
        }

        public bool IsSoundRecognised
        {
            get { return _isSoundRecognised; }
            set { Set(() => IsSoundRecognised, ref _isSoundRecognised, value); }
        }

        public string SoundText
        {
            get { return _soundText; }
            set { Set(() => SoundText, ref _soundText, value); }
        }

        public MainPageViewModel(IPlayersRepository playersRepository, IConfigurationRepository configurationRepository,
            Func<string, IDartGame> dartGameFactory, Func<string, IStatisticsCalculationService> statisticsFactory,
            ICommentaryPlayer commentaryPlayer, IDialogService dialogService) : base(commentaryPlayer,
            dialogService)
        {
            _playersRepository = playersRepository;
            _configurationRepository = configurationRepository;
            _dartGameFactory = dartGameFactory;
            _statisticsFactory = statisticsFactory;

            InitialiseStandingDataAndSound();

            InitialiseSubscriptions();

            InitaliseCommands();

            SetupMessageBus();
        }

        private void SetupMessageBus()
        {
            MessageBus.Current.Listen<GameConfiguration>().Subscribe(ChangeGameConfiguration);
        }

        private void ChangeGameConfiguration(GameConfiguration config)
        {
            if (config != null)
            {
                ConfigurationData = config;
                PlaySound = ConfigurationData.PlaySounds;
            }
        }

        private void InitaliseCommands()
        {
            StartGameCommand = ReactiveCommand.Create();
            StartGameCommand.Subscribe(arg => StartGameCommandHandler());

            ResetCommand = ReactiveCommand.Create();
            ResetCommand.Subscribe(arg => ResetCommandHandler());

            StopGameCommand = ReactiveCommand.Create();
            StopGameCommand.Subscribe(arg => StopGameWindowHandler());

            OpenNewGameWindow = ReactiveCommand.Create();
            OpenNewGameWindow.Subscribe(arg => OpenNewGameWindowHandler());

            FlipCommand = ReactiveCommand.Create();
            FlipCommand.Subscribe(arg => FlipCommandHandler());

            UndoCommand = ReactiveCommand.Create();
            UndoCommand.Subscribe(arg => UndoCommandHandler());

            AcceptScoreCommand = ReactiveCommand.Create();
            AcceptScoreCommand.Subscribe(arg => AcceptScoreCommandHandler());

            RejectScoreCommand = ReactiveCommand.Create();
            RejectScoreCommand.Subscribe(arg => RejectScoreCommandHandler());
        }

        private void StopGameWindowHandler()
        {
            EndGameCommandHandler();
        }

        private void OpenNewGameWindowHandler()
        {
            PlayersList = new ReactiveList<PlayerDetails>(_playersRepository.GetAll());

            SetUpPlayerConfigData();
        }

        private void EndGameCommandHandler()
        {
            DialogService.ShowMessage("This will clear the current game are you sure?.", "Darts Score Master", YesCommandHandler);
        }

        private void StartNextGame()
        {
            if (!MatchWon)
            {
                StartNewGame();
            }
        }

        private void SetIsBust(bool state)
        {
            _participatingPlayers.ForEach(player => player.IsBust = state);
        }

        private void SetIsGameShot(bool state)
        {
            _participatingPlayers.ForEach(player => player.IsGameShot = state);
        }

        private void AcceptOrRejectHelper()
        {
            MessageBus.Current.SendMessage(Visibility.Collapsed);
            ClearDarts();
            _dartTotal = 0;
            SetIsBust(false);
            AnalyseScoreForHints();
            IsBoardEnabled = true;
        }

        private void AnalyseScoreForHints()
        {
            ResetCheckOutHints();

            if (_configurationData.ShowCheckoutHints && SelectedGame.ShowCheckOutHints &&
                GetCurrentPlayer().DartsToThrow > 0)
            {
                var gameParameters = GetGameParameters();

                CheckoutHints = _currentGameService.GetCheckOutHints(gameParameters);

                if (CheckoutHints.Any())
                {
                    HintList = CheckoutHints.Select(hint => new HintList {HintDarts = hint.Item2.Split(' ').Select
                        ((dart, index) => new Hint {HintText = dart, HintImage = GetImage(index)}).ToList()}).ToList();

                    // If user can still checkout remove hints that are more than current number of darts
                    if (GetCurrentPlayer().DartsToThrow < 3 &&
                        HintList.Any(hint => hint.HintDarts.Count <= GetCurrentPlayer().DartsToThrow))
                    {
                        HintList = HintList.Where(hint => hint.HintDarts.Count <= GetCurrentPlayer().DartsToThrow).ToList();
                    }
                }
            }
        }

        private BitmapImage GetImage(int index)
        {
            return GetCurrentPlayer().DartsToThrow > index ? GetCurrentPlayer().DartImage : null;
        }

        private void ResetCheckOutHints()
        {
            CheckoutHints = new List<Tuple<int, string>>();
            HintList = new List<HintList>();
        }

        private DartGameParameters GetGameParameters()
        {
            return new DartGameParameters
            {
                Players = ParticipatingPlayers,
                CurrentPlayer = _currentPlayer,
                DartTotal = _dartTotal,
                DartScore = DartScore,
                IsBust = GetCurrentPlayer().IsBust,
                CurrentGameId = _currentGameId,
                CurrentTurn = _currentTurn
            };
        }

        private void ClearDarts()
        {
            Throw1 = string.Empty;
            Throw2 = string.Empty;
            Throw3 = string.Empty;
            TotalThrow = string.Empty;
        }

        private void RejectScoreCommandHandler()
        {
            IsAcceptOpen = false;

            SetCurrentPlayer(_currentPlayer);

            _currentGameService.RejectScore(GetGameParameters());

            AcceptOrRejectHelper();
        }

        private void AcceptScoreCommandHandler()
        {
            IsAcceptOpen = false;

            var gameParameters = GetGameParameters();

            var isGameOver = CheckGameOverHelper();

            gameParameters.IsGameOver = isGameOver;

            _currentGameService.AcceptScore(gameParameters);

            if (isGameOver)
            {
                ProcessWonGame();
            }
            else
            {
                SetNextPlayer();

                var score = gameParameters.IsBust ? 0 : gameParameters.DartTotal;

                if (SelectedGame.Id != GameType.Cricket && ParticipatingPlayers[_currentPlayer].CurrentScore <= 180 &&
                    ParticipatingPlayers[_currentPlayer].CurrentScore > 1)
                {
                    CommentaryPlayer.Play(new Commentary
                    {
                        PlaySounds = _configurationData.PlaySounds,
                        SoundFiles = new[]
                        {
                            $"s{score}.wav",
                            $"yr{ParticipatingPlayers[_currentPlayer].CurrentScore}.wav"
                        },

                        SoundTexts = new[]
                        {
                            score == 0 ? "NO SCORE": score.ToString(),
                            $"{ParticipatingPlayers[_currentPlayer].NickName} YOU REQUIRE {ParticipatingPlayers[_currentPlayer].CurrentScore}"
                        }
                    });
                }
                else
                {
                    CommentaryPlayer.Play(new Commentary
                    {
                        PlaySounds = _configurationData.PlaySounds,
                        SoundFiles = new[] {$"s{score}.wav"},
                        SoundTexts = new[] { score == 0 ? "NO SCORE" : score.ToString() }
                    });
                }
            }

            AcceptOrRejectHelper();
        }

        private void SetNextPlayer()
        {
            _currentPlayer++;
            _currentTurn++;

            if (_currentPlayer >= ParticipatingPlayers.Count)
            {
                _currentPlayer = 0;
            }

            SetCurrentPlayer(_currentPlayer);
        }

        private void ProcessWonGame()
        {
            var soundToPlay = "gs.wav";
            var soundToSay = "GAME SHOT";

            var winners = _currentGameService.GetVictoriousPlayers(GetGameParameters());

            GetCurrentPlayer().IsGameShot = true;

            ResetCheckOutHints();

            // Determine if any other player is a joint winner
            var playersThatHaveWon =
                ParticipatingPlayers.Where(m => m.CurrentScore == winners.First().CurrentScore
                                                && m.SetsWon == winners.First().SetsWon &&
                                                m.LegsWon == winners.First().LegsWon).ToList();

            foreach (var victoriousPlayer in playersThatHaveWon)
            {
                victoriousPlayer.IsWinner = true;

                // Update current score
                victoriousPlayer.LegsWon++;

                victoriousPlayer.CummulativeLegsWon++;
            }

            // Update legs played for all players
            foreach (var player in ParticipatingPlayers)
            {
                player.LegsPlayed++;
                player.CurrentDartCount = 0;
            }

            if (playersThatHaveWon.First().LegsWon == SelectedLeg)
            {
                soundToPlay = "gss.wav";
                soundToSay = "GAME SHOT AND THE SET";

                foreach (var player in ParticipatingPlayers)
                {
                    player.LegsWon = 0;
                }

                // Update current score
                foreach (var victoriousPlayer in playersThatHaveWon)
                {
                    victoriousPlayer.SetsWon++;
                }

                if (playersThatHaveWon.First().SetsWon == SelectedSet)
                {
                    HandleMatchWon();
                    soundToPlay = "gsm.wav";
                    soundToSay = "GAME SHOT AND THE MATCH";
                }
            }

            foreach (var player in ParticipatingPlayers)
            {
                player.LeaderMessage = string.Empty;
            }

            StartNextGame();

            var nextPlayerSound = playersThatHaveWon.First().SetsWon == SelectedSet ? null : $"tfplayer{_currentPlayer + 1}.wav";

            var nextPlayerText = playersThatHaveWon.First().SetsWon == SelectedSet
                ? null
                : $"{GetCurrentPlayerName()} TO THROW FIRST";

            CommentaryPlayer.Play(new Commentary
            {
                PlaySounds = _configurationData.PlaySounds,
                SoundFiles = new[] { soundToPlay, nextPlayerSound },
                SoundTexts = new[] { soundToSay, nextPlayerText }
            });
        }

        private void HandleMatchWon()
        {
            IsGameRunning = false;

            var playersThatHaveWon = ParticipatingPlayers.Where(m => m.IsWinner).ToList();

            if (ParticipatingPlayers.Count == 1)
            {
                playersThatHaveWon.First().WinMessage = "PRACTICE COMPLETE";
            }
            else if (playersThatHaveWon.Count == 1)
            {
                playersThatHaveWon.First().WinMessage = "MATCH WON";
            }
            else
            {
                foreach (var player in playersThatHaveWon)
                {
                    player.WinMessage = "MATCH DRAWN";
                }
            }

            _matchWon = true;

            var playersToCalculate = ParticipatingPlayers.Where(m => m.PlayerDetails != null).ToList();

            _currentGameService.CloseMatch(GetGameParameters());

            _currentStatisticsService.CalculatePlayerStatistics(SelectedGame.Id, playersToCalculate);

            MessageBus.Current.SendMessage(playersToCalculate);
        }

        private bool CheckGameOverHelper()
        {
            return _currentGameService.IsGameOver(GetGameParameters());
        }

        private bool CheckValidScoreHelper()
        {
            return CheckGameOverHelper() || _currentGameService.IsValidScore(GetGameParameters());
        }

        private bool CheckTurnOverHelper()
        {
            return AreDartsThrown() || CheckGameOverHelper() ||
                   !CheckValidScoreHelper();
        }

        private void InitaliseFlags()
        {
            SetIsBust(false);
            IsGameRunning = true;
            IsBoardEnabled = true;
            Enabled = true;

            _dartTotal = 0;
            ResetCheckOutHints();
            MatchWon = false;
            _currentTurn = 0;
            _currentGameId = Guid.NewGuid();
        }

        private void InitialiseAllPlayers()
        {
            foreach (var participatingPlayer in ParticipatingPlayers)
            {
                participatingPlayer.InitPlayer(SelectedSet, SelectedLeg,
                    SelectedGame.GetStartingScore(participatingPlayer.PlayerDetails), _matchWon);
            }
        }

        private void StartNewGame()
        {
            MatchDetails = SelectedGame.Name;
            _currentGameService = _dartGameFactory(SelectedGame.ClassId);
            _currentStatisticsService = _statisticsFactory(SelectedGame.ClassId);
            PlayerTemplate = (DataTemplate)Application.Current.Resources[SelectedGame.PlayerTemplateName];

            InitaliseFlags();

            // Hide all the darts
            MessageBus.Current.SendMessage(Visibility.Collapsed);

            ClearDarts();

            InitialiseAllPlayers();

            SetCurrentPlayer(_nextUpIndex);

            RotateStartingPlayer();

            AnalyseScoreForHints();

            SetLeaderMessage();
        }

        private void SetLeaderMessage()
        {
            // 1 player practice then no leader
            if (ParticipatingPlayers.Count == 1)
            {
                return;
            }

            // Check to see if there is a leader.
            var currentHighScore =
                ParticipatingPlayers.Select(participatingPlayer => participatingPlayer.GetCurrentGameScore())
                    .Concat(new[] { 0 })
                    .Max();

            var shouldSetLeader = ParticipatingPlayers.Count(m => m.GetCurrentGameScore() >= currentHighScore) == 1;

            foreach (var player in ParticipatingPlayers)
            {
                player.LeaderMessage = player.GetCurrentGameScore() >= currentHighScore && shouldSetLeader
                    ? "LEADER"
                    : string.Empty;
            }
        }


        private void RotateStartingPlayer()
        {
            _nextUpIndex++;

            if (_nextUpIndex == ParticipatingPlayers.Count)
            {
                _nextUpIndex = 0;
            }
        }

        private void StopGame()
        {
            ParticipatingPlayers.Clear();
            SetIsBust(false);
            SetIsGameShot(false);
            IsGameRunning = false;
            IsBoardEnabled = false;
            Enabled = false;
            ClearDarts();
            _dartTotal = 0;
            ResetCheckOutHints();
            MatchDetails = string.Empty;

            foreach (var participatingPlayer in ParticipatingPlayers)
            {
                participatingPlayer.InitPlayer(SelectedSet, SelectedLeg, SelectedGame.StartingScore, true);

                participatingPlayer.RemoveScore();
            }

            // Hide all the darts
            MessageBus.Current.SendMessage(Visibility.Collapsed);
        }

        private void FlipCommandHandler()
        {
            ShowSimpleBoard = !ShowSimpleBoard;

            _configurationData.ShowSimpleBoard = ShowSimpleBoard;

            SetFlipButtonImage();

            MessageBus.Current.SendMessage(true);
        }

        private void SetFlipButtonImage()
        {
            var imageName = ShowSimpleBoard
                ? "ms-appx:///Assets/board.png"
                : "ms-appx:///Assets/bigboard.png";

            FlipImageSource = new BitmapImage(new Uri(imageName, UriKind.Absolute));
        }

        private void UndoCommandHandler()
        {
            IsAcceptOpen = false;
            
            // If no one has scored we are at the start
            if (!_currentGameService.HasAnyOneScored(ParticipatingPlayers.ToList()))
            {
                return;
            }

            if (ParticipatingPlayers[_currentPlayer].DartsToThrow < 3)
            {
                // Score has not yet been accepted so just cancel the dialog
                RejectScoreCommandHandler();
                return;
            }

            _currentPlayer--;

            if (_currentPlayer < 0)
            {
                _currentPlayer = ParticipatingPlayers.Count - 1;
            }

            if (!ParticipatingPlayers[_currentPlayer].UndoItems.Any())
            {
                return;
            }

            _currentGameService.Undo(GetGameParameters());

            ParticipatingPlayers[_currentPlayer].RemoveDartScores();

            var turnNumber =
                ParticipatingPlayers[_currentPlayer].Scores[ParticipatingPlayers[_currentPlayer].Scores.Count - 1]
                    .TurnNumber;

            for (var index = ParticipatingPlayers[_currentPlayer].Scores.Count - 1; index >= 0; index--)
            {
                var score = ParticipatingPlayers[_currentPlayer].Scores[index];

                if (score.TurnNumber == turnNumber)
                {
                    ParticipatingPlayers[_currentPlayer].Scores.Remove(score);
                }
            }

            SetCurrentPlayer(_currentPlayer);

            AnalyseScoreForHints();
        }

        private void ResetCommandHandler()
        {
            SelectedLeg = 1;
            SelectedSet = 1;
            SelectedPlayers = 1;

            SelectedGame = Games.First(m => m.Id == _configurationData.GameType);

            foreach (var configuredPlayer in ConfiguredPlayers)
            {
                configuredPlayer.PlayerDetails = null;
            }
        }

        private void StartGameCommandHandler()
        {
            IsFlyoutClosed = false;

            _currentPlayer = 0;

            SaveNewConfigData();

            SetUpGame();

            SetUpPlayerConfigData();

            CommentaryPlayer.Play(new Commentary
            {
                PlaySounds = _configurationData.PlaySounds,
                SoundFiles = new[]
                {
                    $"tfplayer{_currentPlayer + 1}.wav"
                },
                SoundTexts = new[] { $"{GetCurrentPlayerName()} TO THROW FIRST" }
            });

            StartNewGame();
        }

        private Player GetCurrentPlayer()
        {
            return _participatingPlayers[_currentPlayer];
        }

        private string GetCurrentPlayerName()
        {
            return GetCurrentPlayer().NickName;
        }

        private bool AreDartsThrown()
        {
            return ParticipatingPlayers[_currentPlayer].DartsToThrow <= 0;
        }

        private void YesCommandHandler(IUICommand command)
        {
            IsAcceptOpen = false;

            StopGame();
        }

        protected void InitialiseSubscriptions()
        {
            this.WhenAny(x => x.IsGameRunning, x => x.Value).Subscribe(ProcessIsGameRunning);

            this.WhenAny(x => x.DartScore, x => x.Value).Subscribe(SetScore);

            this.WhenAny(x => x.ConfigurationData, x => x.Value).Subscribe(ChangeGameConfiguration);

            this.WhenAny(x => x.SelectedPlayers, x => x.Value).Skip(1).Subscribe(InitialiseConfiguredPlayers);

            this.WhenAnyValue(x => x.IsGameRunning).Subscribe(_ => { IsUndoEnabled = IsGameRunning; });
        }

        private void InitialiseConfiguredPlayers(int selectedPlayerCount)
        {
            ConfiguredPlayers.Clear();

            for (var i = 0; i < selectedPlayerCount; i++)
            {
                ConfiguredPlayers.Add(new Player(i));
            }
        }

        private void ProcessIsGameRunning(bool isGameRunning)
        {
            GameButtonCaption = IsGameRunning ? "STOP" : "NEW";
        }

        private void SetScore(Score score)
        {
            if (DartScore == null || !IsGameRunning || AreDartsThrown())
            {
                return;
            }

            SetIsBust(false);
            SetIsGameShot(false);

            var participatingPlayer = ParticipatingPlayers[_currentPlayer];

            participatingPlayer.LastDarts.Add(score);

            participatingPlayer.DartsToThrow--;

            _dartTotal += _currentGameService.ProcessTotal(GetGameParameters());

            participatingPlayer.CurrentDartCount++;

            Throw1 = participatingPlayer.GetDart(1);
            Throw2 = participatingPlayer.GetDart(2);
            Throw3 = participatingPlayer.GetDart(3);

            TotalThrow = _currentGameService.ProcessScore(GetGameParameters());

            AnalyseScoreForHints();

            if (CheckTurnOverHelper())
            {
                IsAcceptOpen = true;

                IsBoardEnabled = false;

                GetCurrentPlayer().IsBust = !CheckValidScoreHelper();
            }
        }

        private void SetUpGame()
        {
            _nextUpIndex = 0;

            InitialiseStartingPlayers();

            InitialiseParticipatingPlayers();
        }

        private void InitialiseParticipatingPlayers()
        {
            ParticipatingPlayers.Clear();

            for (var i = 1; i <= SelectedPlayers; i++)
            {
                var player = new Player(i);

                // Set up the player details for each participant
                if (ConfiguredPlayers.Count >= i)
                {
                    player.CurrentScore = SelectedGame.GetStartingScore(ConfiguredPlayers[i - 1].PlayerDetails);
                    player.PlayerDetails = ConfiguredPlayers[i - 1].PlayerDetails;
                }
                else
                {
                    player.CurrentScore = SelectedGame.GetStartingScore(null);
                }

                ParticipatingPlayers.Add(player);
            }
        }

        private void InitialiseStartingPlayers()
        {
            if (SelectedPlayers == 0)
            {
                SelectedPlayers = 2;
            }
        }

        private async void InitialiseStandingDataAndSound()
        {
            await LoadConfigData();

            IsGameRunning = false;
            _dartTotal = 0;
            ClearDarts();

            if (SelectedSet == 0)
            {
                SelectedSet = 1;
            }

            if (SelectedLeg == 0)
            {
                SelectedLeg = 1;
            }

            if (SelectedPlayers == 0)
            {
                SelectedPlayers = 2;
            }

            ShowSimpleBoard = _configurationData.ShowSimpleBoard;

            SetFlipButtonImage();
        }

        private void SaveNewConfigData()
        {
            _configurationData.GameType = SelectedGame.Id;
            _configurationData.Sets = SelectedSet;
            _configurationData.LegsPerSet = SelectedLeg;
            _configurationData.PlayerIndexes = new List<Guid>();
            _configurationData.PlayerIds = new List<int>();
            _configurationData.PlayerCount = SelectedPlayers;

            foreach (var player in ConfiguredPlayers.Where(player => player.PlayerDetails != null))
            {
                _configurationData.PlayerIndexes.Add(player.PlayerDetails.Id);
                _configurationData.PlayerIds.Add(player.Number);
            }

            SaveConfigData();
        }

        private async void SaveConfigData()
        {
            _configurationRepository.Add(_configurationData);
            await _configurationRepository.Save();
        }

        private async Task LoadConfigData()
        {
            ConfigurationData = await _configurationRepository.GetAll();

            SelectedGame = Games.First(m => m.Id == _configurationData.GameType);
            SelectedSet = _configurationData.Sets;
            SelectedLeg = _configurationData.LegsPerSet;
            SelectedPlayers = _configurationData.PlayerCount < 1 ? 1 : _configurationData.PlayerCount;
            IsSoundSupportEnabled = _configurationData.EnableSoundRecognition;

            InitialiseConfiguredPlayers(_selectedPlayers);
        }

        private void SetUpPlayerConfigData()
        {
            for (var i = 0; i < _configurationData.PlayerIndexes.Count; i++)
            {
                ConfiguredPlayers[_configurationData.PlayerIds[i]].PlayerDetails =
                    PlayersList.FirstOrDefault(m => m.Id == _configurationData.PlayerIndexes[i]);
            }
        }

        private void SetCurrentPlayer(int currentPlayer)
        {
            CurrentPlayer = currentPlayer;

            if (ParticipatingPlayers[_currentPlayer].PlayerDetails != null)
            {
                SelectedPlayerImage = ParticipatingPlayers[_currentPlayer].PlayerDetails.PlayerImageDefinition.Source;
            }

            foreach (var player in ParticipatingPlayers)
            {
                player.IsInPlay = false;
                player.UnHighlight();
                player.DartsToThrow = 3;
            }

            var flightIndex = ParticipatingPlayers[_currentPlayer].PlayerDetails == null
                ? currentPlayer + 1 : ParticipatingPlayers[_currentPlayer].PlayerDetails.SelectedFlight.Index;

            DartImageName = $"dartinboard{flightIndex}.png";
            ParticipatingPlayers[_currentPlayer].IsInPlay = true;
            ParticipatingPlayers[_currentPlayer].IsWinner = false;
            ParticipatingPlayers[_currentPlayer].Highlight();
        }

        public async void Activate(object parameter)
        {
            await LoadConfigData();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}