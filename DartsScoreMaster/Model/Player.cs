using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DartsScoreMaster.ViewModels;
using ReactiveUI;

namespace DartsScoreMaster.Model
{
    public class Player : BaseViewModel
    {
        private int _number;
        private int _currentScore;
        private ReactiveList<Score> _scores;
        private ObservableCollection<Score> _turnScores;
        private string _name;
        private bool _isInPlay;
        private int _dartsToThrow;
        private bool _showFirstDart;
        private bool _showThirdDart;
        private bool _isWinner;
        private ReactiveList<Score> _lastDarts;
        private int _setsWon;
        private int _legsWon;
        private string _setScore;
        private string _legScore;
        private int _legsPerSet;
        private int _sets;
        private bool _showSecondDart;
        private string _winMessage;
        private string _leaderMessage;
        private PlayerDetails _playerDetails;
        private int _legsPlayed;
        private int _cummulativeLegsWon;
        private int _hits20;
        private int _hits19;
        private int _hits18;
        private int _hits17;
        private int _hits16;
        private int _hits15;
        private int _hitsBull;

        private SolidColorBrush _hits20Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hits19Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hits18Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hits17Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hits16Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hits15Color = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _hitsBullColor = new SolidColorBrush(Colors.Red);

        private Guid _uniqueId;
        private ReactiveList<UndoItem> _undoItems;
        private int _interimScore;
        private BitmapImage _twentyImage;
        private BitmapImage _nineteenImage;
        private BitmapImage _eighteenImage;
        private BitmapImage _seventeenImage;
        private BitmapImage _sixteenImage;
        private BitmapImage _fifteenImage;
        private BitmapImage _bullImage;
        private int _currentDartCount;
        private Brush _blackBoardColor;
        private bool _isBust;
        private bool _isGameShot;

        private void SetupObservables()
        {
            this.WhenAny(x => x.DartsToThrow, y => y.IsInPlay,
                (darts, inPlay) => inPlay.Value && darts.Value >= 1)
                 .Subscribe(UpdateFirstDart);

            this.WhenAny(x => x.DartsToThrow, y => y.IsInPlay,
                 (darts, inPlay) => inPlay.Value && darts.Value >= 2)
                  .Subscribe(UpdateSecondDart);

            this.WhenAny(x => x.DartsToThrow, y => y.IsInPlay,
               (darts, inPlay) => inPlay.Value && darts.Value >= 3)
                .Subscribe(UpdateThirdDart);

            this.WhenAny(x => x.LegsWon, y => y.SetsWon,
                (x, y) => true)
                .Subscribe(x => UpdateScore());
        }

        private void UpdateFirstDart(bool showDart)
        {
            ShowFirstDart = showDart;
        }

        private void UpdateSecondDart(bool showDart)
        {
            ShowSecondDart = showDart;
        }

        private void UpdateThirdDart(bool showDart)
        {
            ShowThirdDart = showDart;
        }

        public void UpdateScore()
        {
            if (Sets == 0 && LegsPerSet == 0)
            {
                RemoveScore();
                return;
            }

            SetScore = "Sets:  " + SetsWon + "/" + Sets;
            LegScore = "Legs:  " + LegsWon + "/" + LegsPerSet;
        }


        public void RemoveDartScores()
        {
            foreach (var dart in LastDarts)
            {
                dart.Value = 0;
            }
        }

        public void RemoveScore()
        {
            SetScore = LegScore = string.Empty;
        }

        public void Highlight()
        {
            BlackBoardColor = new SolidColorBrush(Colors.Yellow);
        }

        public void UnHighlight()
        {
            BlackBoardColor = new SolidColorBrush(Colors.White);
        }

        public void InitPlayer(int sets, int legs, int startingScore, bool resetScores)
        {
            UnHighlight();

            Sets = sets;
            LegsPerSet = legs;
            DartsToThrow = 0;
            CurrentDartCount = 0;

            LastDarts.Clear();
            UndoItems.Clear();

            if (resetScores)
            {
                TurnScores.Clear();
                SetsWon = 0;
                LegsWon = 0;
                LegsPlayed = 0;
                CummulativeLegsWon = 0;
            }

            IsWinner = false;
            CurrentScore = startingScore;
            InterimScore = CurrentScore;
            WinMessage = string.Empty;
            LeaderMessage = string.Empty;
            UpdateScore();

            TwentyImage = new BitmapImage(new Uri("ms-appx:///Assets/twenty.png", UriKind.Absolute));
            NineteenImage = new BitmapImage(new Uri("ms-appx:///Assets/nineteen.png", UriKind.Absolute));
            EighteenImage = new BitmapImage(new Uri("ms-appx:///Assets/eighteen.png", UriKind.Absolute));
            SeventeenImage = new BitmapImage(new Uri("ms-appx:///Assets/seventeen.png", UriKind.Absolute));
            SixteenImage = new BitmapImage(new Uri("ms-appx:///Assets/sixteen.png", UriKind.Absolute));
            FifteenImage = new BitmapImage(new Uri("ms-appx:///Assets/fifteen.png", UriKind.Absolute));
            BullImage = new BitmapImage(new Uri("ms-appx:///Assets/bull.png", UriKind.Absolute));
        }

        public bool ShowFirstDart
        {
            get { return _showFirstDart; }
            set
            {
                Set(() => ShowFirstDart, ref _showFirstDart, value);
            }
        }

        public bool ShowSecondDart
        {
            get { return _showSecondDart; }
            set
            {
                Set(() => ShowSecondDart, ref _showSecondDart, value);
            }
        }

        public int LegsPerSet
        {
            get { return _legsPerSet; }
            set
            {
                Set(() => LegsPerSet, ref _legsPerSet, value);
            }
        }

        public int Sets
        {
            get { return _sets; }
            set
            {
                Set(() => Sets, ref _sets, value);
            }
        }

        public bool ShowThirdDart
        {
            get { return _showThirdDart; }
            set
            {
                Set(() => ShowThirdDart, ref _showThirdDart, value);
            }
        }

        public int CurrentDartCount
        {
            get { return _currentDartCount; }
            set
            {
                Set(() => CurrentDartCount, ref _currentDartCount, value);
            }
        }

        public string SetScore
        {
            get { return _setScore; }
            set
            {
                Set(() => SetScore, ref _setScore, value);
            }
        }

        public string LegScore
        {
            get { return _legScore; }
            set
            {
                Set(() => LegScore, ref _legScore, value);
            }
        }

        public bool IsInPlay
        {
            get { return _isInPlay; }
            set
            {
                Set(() => IsInPlay, ref _isInPlay, value);
            }
        }

        public int DartsToThrow
        {
            get { return _dartsToThrow; }
            set
            {
                Set(() => DartsToThrow, ref _dartsToThrow, value);
            }
        }

        public string Name
        {
            get { return PlayerDetails == null ? _name : PlayerDetails.Name; }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public Guid UniqueId
        {
            get { return _uniqueId; }
            set
            {
                Set(() => UniqueId, ref _uniqueId, value);
            }
        }

        public int SetsWon
        {
            get { return _setsWon; }
            set
            {
                Set(() => SetsWon, ref _setsWon, value);
            }
        }

        public int LegsWon
        {
            get { return _legsWon; }
            set
            {
                Set(() => LegsWon, ref _legsWon, value);
            }
        }

        public int LegsPlayed
        {
            get { return _legsPlayed; }
            set
            {
                Set(() => LegsPlayed, ref _legsPlayed, value);
            }
        }

        public int CummulativeLegsWon
        {
            get { return _cummulativeLegsWon; }
            set
            {
                Set(() => CummulativeLegsWon, ref _cummulativeLegsWon, value);
            }
        }

        public int Number
        {
            get { return _number; }
            set
            {
                Set(() => Number, ref _number, value);
            }
        }

        public bool IsGameShot
        {
            get { return _isGameShot; }
            set { Set(() => IsGameShot, ref _isGameShot, value); }
        }

        public bool IsBust
        {
            get { return _isBust; }
            set { Set(() => IsBust, ref _isBust, value); }
        }

        public int CurrentScore
        {
            get { return _currentScore; }
            set
            {
                Set(() => CurrentScore, ref _currentScore, value);
            }
        }

        public int Hits20
        {
            get { return _hits20; }
            set
            {
                Set(() => Hits20, ref _hits20, value);
            }
        }

        public SolidColorBrush Hits20Color
        {
            get { return _hits20Color; }
            set
            {
                Set(() => Hits20Color, ref _hits20Color, value);
            }
        }

        public int Hits19
        {
            get { return _hits19; }
            set
            {
                Set(() => Hits19, ref _hits19, value);
            }
        }

        public SolidColorBrush Hits19Color
        {
            get { return _hits19Color; }
            set
            {
                Set(() => Hits19Color, ref _hits19Color, value);
            }
        }

        public int InterimScore
        {
            get { return Math.Max(_interimScore, 0); }
            set
            {
                Set(() => InterimScore, ref _interimScore, value);
            }
        }

        public SolidColorBrush Hits18Color
        {
            get { return _hits18Color; }
            set
            {
                Set(() => Hits18Color, ref _hits18Color, value);
            }
        }

        public SolidColorBrush Hits17Color
        {
            get { return _hits17Color; }
            set
            {
                Set(() => Hits17Color, ref _hits17Color, value);
            }
        }

        public SolidColorBrush Hits16Color
        {
            get { return _hits16Color; }
            set
            {
                Set(() => Hits16Color, ref _hits16Color, value);
            }
        }

        public SolidColorBrush Hits15Color
        {
            get { return _hits15Color; }
            set
            {
                Set(() => Hits15Color, ref _hits15Color, value);
            }
        }

        public SolidColorBrush HitsBullColor
        {
            get { return _hitsBullColor; }
            set
            {
                Set(() => HitsBullColor, ref _hitsBullColor, value);
            }
        }

        public int Hits18
        {
            get { return _hits18; }
            set
            {
                Set(() => Hits18, ref _hits18, value);
            }
        }

        public int Hits17
        {
            get { return _hits17; }
            set
            {
                Set(() => Hits17, ref _hits17, value);
            }
        }

        public int Hits16
        {
            get { return _hits16; }
            set
            {
                Set(() => Hits16, ref _hits16, value);
            }
        }

        public int Hits15
        {
            get { return _hits15; }
            set
            {
                Set(() => Hits15, ref _hits15, value);
            }
        }

        public int HitsBull
        {
            get { return _hitsBull; }
            set
            {
                Set(() => HitsBull, ref _hitsBull, value);
            }
        }

        public ReactiveList<Score> LastDarts
        {
            get { return _lastDarts; }
            set
            {
                Set(() => LastDarts, ref _lastDarts, value);
            }
        }

        public ReactiveList<Score> Scores
        {
            get { return _scores; }
            set
            {
                Set(() => Scores, ref _scores, value);
            }
        }
        
        public ReactiveList<UndoItem> UndoItems
        {
            get { return _undoItems; }
            set
            {
                Set(() => UndoItems, ref _undoItems, value);
            }
        }

        public ObservableCollection<Score> TurnScores
        {
            get { return _turnScores; }
            set
            {
                Set(() => TurnScores, ref _turnScores, value);
            }
        }

        public bool IsWinner
        {
            get { return _isWinner; }
            set
            {
                Set(() => IsWinner, ref _isWinner, value);
            }
        }

        public string WinMessage
        {
            get { return _winMessage; }
            set
            {
                Set(() => WinMessage, ref _winMessage, value);
            }
        }

        public string LeaderMessage
        {
            get { return _leaderMessage; }
            set
            {
                Set(() => LeaderMessage, ref _leaderMessage, value);
            }
        }

        public PlayerDetails PlayerDetails
        {
            get { return _playerDetails; }
            set
            {
                Set(() => PlayerDetails, ref _playerDetails, value);
            }
        }

        public BitmapImage DartImage
        {
            get
            {
                if (PlayerDetails == null)
                {
                    return
                        new BitmapImage(new Uri($"ms-appx:///Assets/scoredart{Number}.png",
                            UriKind.Absolute));
                }

                return PlayerDetails.SelectedFlight.Image;
            }
        }

        public Brush BlackBoardColor
        {
            get { return _blackBoardColor; }
            set
            {
                Set(() => BlackBoardColor, ref _blackBoardColor, value);
            }
        }

        public BitmapImage TwentyImage
        {
            get { return _twentyImage; }
            set
            {
                Set(() => TwentyImage, ref _twentyImage, value);
            }
        }

        public BitmapImage NineteenImage
        {
            get { return _nineteenImage; }
            set
            {
                Set(() => NineteenImage, ref _nineteenImage, value);
            }
        }

        public BitmapImage EighteenImage
        {
            get { return _eighteenImage; }
            set
            {
                Set(() => EighteenImage, ref _eighteenImage, value);
            }
        }

        public BitmapImage SeventeenImage
        {
            get { return _seventeenImage; }
            set
            {
                Set(() => SeventeenImage, ref _seventeenImage, value);
            }
        }

        public BitmapImage SixteenImage
        {
            get { return _sixteenImage; }
            set
            {
                Set(() => SixteenImage, ref _sixteenImage, value);
            }
        }

        public BitmapImage FifteenImage
        {
            get { return _fifteenImage; }
            set
            {
                Set(() => FifteenImage, ref _fifteenImage, value);
            }
        }

        public BitmapImage BullImage
        {
            get { return _bullImage; }
            set
            {
                Set(() => BullImage, ref _bullImage, value);
            }
        }

        public string NickName
        {
            get { return PlayerDetails == null ? Name : PlayerDetails.NickName; }
        }

        public Dictionary<int, int> OldHits { get; } = new Dictionary<int, int>();

        public Player(int number)
        {
            UniqueId = Guid.NewGuid();

            Number = number;
            Reset();

            SetupObservables();

        }

        public void DiscardLastDarts()
        {
            MergeLastDarts(true, null, 0);
        }

        public void MergeLastDarts(bool discard, Leg leg, int currentTurn)
        {
            if (!discard)
            {
                foreach (var dart in LastDarts)
                {
                    dart.ScoredInLeg = leg;
                    dart.TurnNumber = currentTurn;
                    Scores.Add(dart);
                }

                var scoreToAdd = new Score
                {
                    TurnNumber = currentTurn,
                    ScoredInLeg = leg,
                    Value = CurrentScore,
                    DartScore = LastDarts.Sum(m => m.Value)
                };

                TurnScores.Insert(0, scoreToAdd);
            }
            else
            {
                CurrentDartCount -= LastDarts.Count;
            }

            LastDarts.Clear();
        }

        public int GetCurrentCummulativeScore()
        {
            return CurrentScore + LastDarts.Sum(dart => dart.Value);
        }

        public int GetCurrentGameScore()
        {
            return (SetsWon * LegsPerSet) + LegsWon;
        }

        private void Reset()
        {
            Name = "PLAYER " + Number;
            Scores = new ReactiveList<Score>();
            LastDarts = new ReactiveList<Score>();
            TurnScores = new ObservableCollection<Score>();
            UndoItems = new ReactiveList<UndoItem>();
        }

        public void ResetCricketScores()
        {
            foreach (var hit in OldHits)
            {
                SetNumberHitCount(hit.Key, hit.Value);
            }

            OldHits.Clear();
        }

        public void AcceptCricketScores()
        {
            OldHits.Clear();
        }

        public string GetDart(int dartIndex)
        {
            dartIndex--;

            return dartIndex >= LastDarts.Count ? string.Empty : LastDarts[dartIndex].Value.ToString();
        }

        public int GetDartBestCheckOut()
        {
            var scoresWithWonLegs = from allScores in Scores
                                    join allScoresGrouped in
                                        Scores.Where(m => m.ScoredInLeg.IsWinner).GroupBy(x => x.ScoredInLeg.Id).ToList() on
                                        allScores.ScoredInLeg.Id equals allScoresGrouped.Key
                                    select allScores;

            var withWonLegs = scoresWithWonLegs as IList<Score> ?? scoresWithWonLegs.ToList();

            if (!withWonLegs.Any())
            {
                return 0;
            }

            return withWonLegs.GroupBy(x => x.ScoredInLeg.Id).Min(x => x.Count());
        }

        public int GetDartCheckOuts(int lowerDartCount, int upperDartCount)
        {
            var scoresWithWonLegs = from allScores in Scores
                                    join allScoresGrouped in
                                        Scores.Where(m => m.ScoredInLeg.IsWinner).GroupBy(x => x.ScoredInLeg.Id).ToList() on
                                        allScores.ScoredInLeg.Id equals allScoresGrouped.Key
                                    select allScores;

            return
                scoresWithWonLegs.GroupBy(x => x.ScoredInLeg.Id)
                    .Count(x => x.Count() >= lowerDartCount && x.Count() <= upperDartCount);
        }

        public void SetNumberHitCount(int numberHit, int value)
        {
            int oldValue;

            switch (numberHit)
            {
                case 20:
                    oldValue = Hits20;
                    Hits20 = value;
                    break;

                case 19:
                    oldValue = Hits19;
                    Hits19 = value;
                    break;

                case 18:
                    oldValue = Hits18;
                    Hits18 = value;
                    break;

                case 17:
                    oldValue = Hits17;
                    Hits17 = value;
                    break;

                case 16:
                    oldValue = Hits16;
                    Hits16 = value;
                    break;

                case 15:
                    oldValue = Hits15;
                    Hits15 = value;
                    break;

                case 50:
                case 25:
                    oldValue = HitsBull;
                    HitsBull = value;
                    break;

                default:
                    throw new Exception($"Value out of rangd, number hit is {numberHit}");
            }

            if (!OldHits.ContainsKey(numberHit))
            {
                OldHits.Add(numberHit, oldValue);
            }
        }

        public int GetNumberHitCount(int numberHit)
        {
            switch (numberHit)
            {
                case 20:
                    return Hits20;

                case 19:
                    return Hits19;

                case 18:
                    return Hits18;

                case 17:
                    return Hits17;

                case 16:
                    return Hits16;

                case 15:
                    return Hits15;

                case 50:
                case 25:
                    return HitsBull;
                default:
                    return 0;
            }
        }

        private SolidColorBrush GetNumberColour(bool allClosed, int hits)
        {
            if (allClosed)
            {
                return new SolidColorBrush(Colors.Black);
            }

            return hits < Cricket.HitsToOpenCloseScore ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
        }

        public void SetNumberColor(int numberHit, bool allClosed)
        {
            switch (numberHit)
            {
                case 20:
                    Hits20Color = GetNumberColour(allClosed, Hits20);

                    TwentyImage = allClosed
                        ? new BitmapImage(new Uri("ms-appx:///Assets/twentyX.png", UriKind.Absolute))
                        : new BitmapImage(new Uri("ms-appx:///Assets/twenty.png", UriKind.Absolute));

                    break;

                case 19:
                    Hits19Color = GetNumberColour(allClosed, Hits19);

                    NineteenImage = allClosed
                      ? new BitmapImage(new Uri("ms-appx:///Assets/nineteenX.png", UriKind.Absolute))
                      : new BitmapImage(new Uri("ms-appx:///Assets/nineteen.png", UriKind.Absolute));
                    break;

                case 18:
                    Hits18Color = GetNumberColour(allClosed, Hits18);

                    EighteenImage = allClosed
                        ? new BitmapImage(new Uri("ms-appx:///Assets/eighteenX.png", UriKind.Absolute))
                        : new BitmapImage(new Uri("ms-appx:///Assets/eighteen.png", UriKind.Absolute));
                    break;

                case 17:
                    Hits17Color = GetNumberColour(allClosed, Hits17);

                    SeventeenImage = allClosed
                        ? new BitmapImage(new Uri("ms-appx:///Assets/seventeenX.png", UriKind.Absolute))
                        : new BitmapImage(new Uri("ms-appx:///Assets/seventeen.png", UriKind.Absolute));
                    break;

                case 16:
                    Hits16Color = GetNumberColour(allClosed, Hits16);

                    SixteenImage = allClosed
                      ? new BitmapImage(new Uri("ms-appx:///Assets/sixteenX.png", UriKind.Absolute))
                      : new BitmapImage(new Uri("ms-appx:///Assets/sixteen.png", UriKind.Absolute));
                    break;

                case 15:
                    Hits15Color = GetNumberColour(allClosed, Hits15);

                    FifteenImage = allClosed
                        ? new BitmapImage(new Uri("ms-appx:///Assets/fifteenX.png", UriKind.Absolute))
                        : new BitmapImage(new Uri("ms-appx:///Assets/fifteen.png", UriKind.Absolute));
                    break;

                case 50:
                case 25:
                    HitsBullColor = GetNumberColour(allClosed, HitsBull);

                    BullImage = allClosed
                    ? new BitmapImage(new Uri("ms-appx:///Assets/bullX.png", UriKind.Absolute))
                    : new BitmapImage(new Uri("ms-appx:///Assets/bull.png", UriKind.Absolute));
                    break;

                default:
                    throw new Exception($"Invalid valiue in numberHit {numberHit}");
            }
        }

        public UndoItem PopUndo()
        {
            var undoItem = UndoItems.LastOrDefault();

            if (undoItem != null)
            {
                UndoItems.Remove(undoItem);
            }

            return undoItem;
        }

        public void ClearUndo()
        {
            UndoItems.Clear();
        }
    }
}