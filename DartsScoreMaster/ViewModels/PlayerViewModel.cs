using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using DartsScoreMaster.Common;
using DartsScoreMaster.Controls.Interfaces;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Services.Interfaces;
using DartsScoreMaster.Tasks.Interfaces;
using ReactiveUI;

namespace DartsScoreMaster.ViewModels
{
    public class PlayerViewModel : VoiceControlViewModel, INavigable
    {
        private readonly IImageLoadTask _imageLoadTask;
        private readonly IPlayersRepository _playersRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private PlayerDetails _playerDetails;
        private ObservableCollection<PlayerDetails> _players;
        private ReactiveList<Flight> _flights;
        private PlayerDetails _selectedPlayer;
        private double _deleteButtonOpacity;
        private double _newButtonOpacity;
        private double _saveButtonOpacity;
        private double _showStatsOpacity;
        private bool _showPlayerView;
        private string _playerButtonCaption;
        private ReactiveList<Game> _gamesForStatistics;
        private DataTemplate _detailTemplate;
        private DataTemplate _headerTemplate;
        private string _column1Header;
        private bool _isCricket;
        private Guid _clickedPlayerId;

        public ReactiveCommand<ImageDefinition> LoadCommand { get; private set; }
        public ReactiveCommand<ImageDefinition> SnapCommand { get; private set; }
        public ReactiveCommand<object> SaveCommand { get; private set; }
        public ReactiveCommand<object> CreateNewCommand { get; private set; }
        public ReactiveCommand<object> DeleteCommand { get; private set; }
        public ReactiveCommand<object> ShowStatsCommand { get; private set; }
        public ReactiveCommand<object> PlayerSummaryCommand { get; private set; }
        public ReactiveCommand<object> PlayerDetailCommand { get; private set; }

        public PlayerViewModel(IImageLoadTask imageLoadTask, IPlayersRepository playersRepository,
            IStatisticsRepository statisticsRepository,
            IConfigurationRepository configurationRepository,
            ICommentaryPlayer commentaryPlayer,
            IDialogService dialogService) : base(commentaryPlayer, dialogService)
        {
            _imageLoadTask = imageLoadTask;
            _playersRepository = playersRepository;
            _statisticsRepository = statisticsRepository;
            _configurationRepository = configurationRepository;

            Initialise();

            SetUpMessageBus();
        }

        private void SetUpMessageBus()
        {
            MessageBus.Current.Listen<List<Player>>().Subscribe(_ => CheckForChangedPlayer());
        }

        private void CheckForChangedPlayer()
        {
            LoadExistingPlayers();
        }

        private void Initialise()
        {
            Flights = StandingData.GetFlights();

            GamesForStatistics = Games;

            LoadCommands();

            CreateNewPlayer();

            LoadExistingPlayers();

            InitialiseSubscriptions();

            ShowPlayerView = true;
        }

        private void ShowSummary(object arg)
        {
            ShowStatsCommand.Execute(false);
            ShowPlayerView = true;
        }

        private void ShowDetail(object arg)
        {
            ShowStatsCommand.Execute(false);
            ShowPlayerView = false;
        }

        private void InitialiseSubscriptions()
        {
            this.WhenAny(x => x.SelectedPlayer,
                    selectedPlayer => selectedPlayer)
                .Subscribe(x => SetDetailView(x.Value));

            this.WhenAny(x => x.SelectedGame,
                    selectedGame => selectedGame.Value)
                .Subscribe(x => ResetStatistics());
        }

        private void SetDetailView(PlayerDetails player)
        {
            if (player != null)
            {
                PlayerDetails = player.Clone();
                PlayerDetails.IsDirty = false;
            }
        }

        private void LoadExistingPlayers()
        {
            Players = new ObservableCollection<PlayerDetails>(_playersRepository.GetAll());

            foreach (var player in Players)
            {
                player.Statistics = _statisticsRepository.GetForPlayer(player.Id);
            }

            ResetStatistics();

            if (Players.Any())
            {
                SelectedPlayer = Players[0];
            }
        }

        private void ResetStatistics()
        {
            IsCricket = SelectedGame.Id == GameType.Cricket;

            foreach (var player in Players)
            {
                player.Statistic = player.Statistics.GetLatestStatisticForGame(SelectedGame.Id) ?? new Statistic();

                switch (SelectedGame.Id)
                {
                    case GameType.T501:
                        player.Statistic.Handicap = player.Handicap501;
                        break;
                    case GameType.T401:
                        player.Statistic.Handicap = player.Handicap401;
                        break;
                    case GameType.T301:
                        player.Statistic.Handicap = player.Handicap301;
                        break;
                    case GameType.T201:
                        player.Statistic.Handicap = player.Handicap201;
                        break;
                    case GameType.T101:
                        player.Statistic.Handicap = player.Handicap101;
                        break;
                    case GameType.Cricket:
                        player.Statistic.Handicap = player.HandicapCricket;
                        break;
                }
            }

            if (SelectedGame.Id == GameType.Cricket)
            {
                Players =
                    new ObservableCollection<PlayerDetails>(
                        Players.OrderByDescending(m => m.Statistic.HighestScore)
                            .ThenByDescending(m => m.Statistic.ThreeDartAverage));

            }
            else
            {
                Players =
                    new ObservableCollection<PlayerDetails>(
                        Players.OrderByDescending(m => m.Statistic.CheckoutPercentage)
                            .ThenByDescending(m => m.Statistic.ThreeDartAverage));

            }
        }

        private void CreateNewPlayer()
        {
            SelectedPlayer = null;
            PlayerDetails = new PlayerDetails {IsDirty = false, SelectedFlight = Flights[0]};
        }

        private void LoadCommands()
        {
            LoadCommand = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var imageDefinition = await _imageLoadTask.PickThumbnailImage();

                return imageDefinition;
            });

            LoadCommand.Subscribe(LoadThumbnailImage);

            SnapCommand = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var camera = new CameraCaptureUI();

                var file = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);

                return await _imageLoadTask.GetImageDefinitionFromFile(file);
            });

            SnapCommand.Subscribe(LoadThumbnailImage);

            SaveCommand =
                ReactiveCommand.Create(this.WhenAny(x => x.PlayerDetails.IsDirty, x => x.PlayerDetails.Name,
                    x => x.PlayerDetails.NickName,
                    (isDirty, name, nickName) =>
                        isDirty.Value && !string.IsNullOrWhiteSpace(name.Value) &&
                        !string.IsNullOrWhiteSpace(nickName.Value)));

            SaveCommand.Subscribe(arg => SaveCommandHandler());

            SaveCommand.CanExecuteObservable.Subscribe(HandleSaveCanExecuteChangedHandler);

            DeleteCommand = ReactiveCommand.Create(this.WhenAny(x => x.PlayerDetails.Id,
                id => id.Value != Guid.Empty));

            DeleteCommand.Subscribe(async arg => await DeleteCommandHandler());

            DeleteCommand.CanExecuteObservable.Subscribe(HandleDeleteCanExecuteChangedHandler);

            CreateNewCommand = ReactiveCommand.Create(this.WhenAnyValue(x => x.PlayerDetails.IsDirty)
                .Select(x => !x).Skip(1));

            CreateNewCommand.Subscribe(arg => CreateCommandHandler());

            CreateNewCommand.CanExecuteObservable.Subscribe(HandleNewCanExecuteChangedHandler);

            ShowStatsCommand = ReactiveCommand.Create();

            ShowStatsCommand.Subscribe(arg =>
            {
                var newId = arg is Guid ? (Guid?) arg : null;

                if (newId.HasValue)
                {
                    ClickedPlayerId = newId.Value;
                }
            });

            ShowStatsCommand.CanExecuteObservable.Subscribe(HandleShowStatsCommandChangedHandler);

            PlayerSummaryCommand = ReactiveCommand.Create();

            PlayerSummaryCommand.Subscribe(ShowSummary);

            PlayerDetailCommand = ReactiveCommand.Create();

            PlayerDetailCommand.Subscribe(ShowDetail);
        }

        private void HandleShowStatsCommandChangedHandler(bool canExecute)
        {
            ShowStatsOpacity = canExecute ? 1.0 : 0.2;
        }

        private void HandleSaveCanExecuteChangedHandler(bool canExecute)
        {
            SaveButtonOpacity = canExecute ? 1.0 : 0.2;
        }

        private void HandleNewCanExecuteChangedHandler(bool canExecute)
        {
            NewButtonOpacity = canExecute ? 1.0 : 0.2;
        }

        private void HandleDeleteCanExecuteChangedHandler(bool canExecute)
        {
            DeleteButtonOpacity = canExecute ? 1.0 : 0.2;
        }

        private async Task DeleteCommandHandler()
        {
            var messageDialog =
                new MessageDialog(
                    "Remove the player completely or just the statistics collected for that player?");

            messageDialog.Commands.Add(new UICommand(
                "Player and Statistics",
                ClearAndDeleteEmployee));

            messageDialog.Commands.Add(new UICommand(
                "Statistics Only",
                ClearStatistics));

            messageDialog.Commands.Add(new UICommand(
                "Cancel"));

            await messageDialog.ShowAsync();
        }

        private async void ClearAndDeleteEmployee(IUICommand command)
        {
            await ClearStatisticsHelper();

            var playerTodelete = Players.FirstOrDefault(m => m.Id == PlayerDetails.Id);

            if (playerTodelete != null)
            {
                Players.Remove(playerTodelete);
            }

            _playersRepository.DeleteById(PlayerDetails.Id);
            _playersRepository.SaveAll();

            CreateNewPlayer();
        }

        private void ClearStatistics(IUICommand command)
        {
            ClearStatisticsHelper();
        }

        private async Task ClearStatisticsHelper()
        {
            var playerTodelete = Players.FirstOrDefault(m => m.Id == PlayerDetails.Id);

            await _statisticsRepository.DeleteByPlayerId(PlayerDetails.Id);

            CheckForChangedPlayer();

            MessageBus.Current.SendMessage(new List<Player> {new Player(0) {PlayerDetails = playerTodelete}});

            ShowStatsCommand.Execute(false);
        }

        private void CreateCommandHandler()
        {
            CreateNewPlayer();
        }

        private void SaveCommandHandler()
        {
            PlayerDetails.IsDirty = false;

            if (PlayerDetails.Id == Guid.Empty)
            {
                PlayerDetails.Id = Guid.NewGuid();
                Players.Add(PlayerDetails);
            }
            else
            {
                var playerToChange = Players.FirstOrDefault(m => m.Id == PlayerDetails.Id);

                playerToChange?.CopyFrom(PlayerDetails);
            }

            // Disconnect object before saving to prevent same object being linked to master detail view.
            _playersRepository.Add(PlayerDetails.Clone());

            _playersRepository.SaveAll();
        }

        private void LoadThumbnailImage(ImageDefinition imageDefinition)
        {
            if (imageDefinition != null)
            {
                PlayerDetails.PlayerImageDefinition = imageDefinition;
            }
        }

        public PlayerDetails PlayerDetails
        {
            get { return _playerDetails; }
            set { Set(() => PlayerDetails, ref _playerDetails, value); }
        }


        public PlayerDetails SelectedPlayer
        {
            get { return _selectedPlayer; }
            set { Set(() => SelectedPlayer, ref _selectedPlayer, value); }
        }

        public ObservableCollection<PlayerDetails> Players
        {
            get { return _players; }
            set { Set(() => Players, ref _players, value); }
        }

        public Guid ClickedPlayerId
        {
            get { return _clickedPlayerId; }
            set { Set(() => ClickedPlayerId, ref _clickedPlayerId, value); }
        }
        public Guid Id { get; set; }
        public ReactiveList<Flight> Flights
        {
            get { return _flights; }
            set { Set(() => Flights, ref _flights, value); }
        }

        public string Column1Header
        {
            get { return _column1Header; }
            set { Set(() => Column1Header, ref _column1Header, value); }
        }

        public bool IsCricket
        {
            get { return _isCricket; }
            set { Set(() => IsCricket, ref _isCricket, value); }
        }

        public double DeleteButtonOpacity
        {
            get { return _deleteButtonOpacity; }
            set { Set(() => DeleteButtonOpacity, ref _deleteButtonOpacity, value); }
        }

        public double NewButtonOpacity
        {
            get { return _newButtonOpacity; }
            set { Set(() => NewButtonOpacity, ref _newButtonOpacity, value); }
        }

        public ReactiveList<Game> GamesForStatistics
        {
            get { return _gamesForStatistics; }
            set { Set(() => GamesForStatistics, ref _gamesForStatistics, value); }
        }

        public double SaveButtonOpacity
        {
            get { return _saveButtonOpacity; }
            set { Set(() => SaveButtonOpacity, ref _saveButtonOpacity, value); }
        }

        public double ShowStatsOpacity
        {
            get { return _showStatsOpacity; }
            set { Set(() => ShowStatsOpacity, ref _showStatsOpacity, value); }
        }

        public string PlayerButtonCaption
        {
            get { return _playerButtonCaption; }
            set { Set(() => PlayerButtonCaption, ref _playerButtonCaption, value); }
        }

        public bool ShowPlayerView
        {
            get { return _showPlayerView; }
            set { Set(() => ShowPlayerView, ref _showPlayerView, value); }
        }

        public DataTemplate DetailTemplate
        {
            get { return _detailTemplate; }
            set
            {
                RaisePropertyChanged();
                Set(() => DetailTemplate, ref _detailTemplate, value);
            }
        }

        public DataTemplate HeaderTemplate
        {
            get { return _headerTemplate; }
            set
            {
                RaisePropertyChanged();
                Set(() => HeaderTemplate, ref _headerTemplate, value);
            }
        }
    
        private async Task LoadConfigData()
        {
            var configurationData = await _configurationRepository.GetAll();

            IsSoundSupportEnabled = configurationData.EnableSoundRecognition;


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