using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using DartsScoreMaster.Model;
using DartsScoreMaster.ViewModels;
using ReactiveUI;
using WinRTXamlToolkit.Controls.Extensions;

namespace DartsScoreMaster.Views
{
    public sealed partial class BigBoard
    {
        private readonly List<Storyboard> _flashers = new List<Storyboard>();

        public static readonly DependencyProperty PlaySoundProperty =
           DependencyProperty.Register("PlaySound", typeof(bool), typeof(BigBoard), new PropertyMetadata(true));

        public bool PlaySound
        {
            get { return (bool)GetValue(PlaySoundProperty); }
            set { SetValue(PlaySoundProperty, value); }
        }

        public List<Tuple<int, int, string>> CheckoutHint
        {
            get { return (List<Tuple<int, int, string>>)GetValue(CheckoutHintProperty); }
            set { SetValue(CheckoutHintProperty, value); }
        }

        public static readonly DependencyProperty CheckoutHintProperty =
            DependencyProperty.Register("CheckoutHint", typeof(List<Tuple<int, int, string>>), typeof(BigBoard), new PropertyMetadata(null, CheckoutHintChanged));

        public Score DartScore
        {
            get { return (Score)GetValue(DartScoreProperty); }
            set { SetValue(DartScoreProperty, value); }
        }

        public static readonly DependencyProperty DartScoreProperty =
            DependencyProperty.Register("DartScore", typeof(Score), typeof(BigBoard), new PropertyMetadata(null));

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register("Enabled", typeof(bool), typeof(BigBoard), new PropertyMetadata(null));

        public string DartImageName
        {
            get { return (string)GetValue(DartImageNameProperty); }
            set { SetValue(DartImageNameProperty, value); }
        }

        public static readonly DependencyProperty DartImageNameProperty =
          DependencyProperty.Register("DartImageName", typeof(string), typeof(BigBoard), new PropertyMetadata(string.Empty, DartImageNameChanged));

        private static void DartImageNameChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var board = source as BigBoard;

            board?.SetImage((string)e.NewValue);
        }

        private static void CheckoutHintChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var board = source as BigBoard;

            if (board != null && e.NewValue != null)
            {
                board.SetCheckoutHints((List<Tuple<int, string>>)e.NewValue);
            }
        }

        private void SetCheckoutHints(IEnumerable<Tuple<int, string>> hints)
        {
            StopFlashers();

            foreach (var hint in hints)
            {
                var firstValue = hint.Item2.Split(' ')[0].ToUpper();

                switch (firstValue)
                {
                    case "BULL":
                        CreateStoryBoard(Bull);
                        break;

                    case "25":
                        CreateStoryBoard(OuterBull);
                        break;

                    default:
                        string controlName;

                        if (firstValue.Contains("D"))
                        {
                            controlName =
                                $"Double{Convert.ToInt32(Regex.Match(firstValue, @"\d+").Value).ToString("00")}";
                            CreateStoryBoard((DependencyObject)dartboard.FindName(controlName));
                        }
                        else if (firstValue.Contains("T"))
                        {
                            controlName =
                                $"Triple{Convert.ToInt32(Regex.Match(firstValue, @"\d+").Value).ToString("00")}";
                            CreateStoryBoard((DependencyObject)dartboard.FindName(controlName));
                        }
                        else
                        {
                            controlName = $"Single{Convert.ToInt32(firstValue).ToString("00")}";
                            CreateStoryBoard((DependencyObject)dartboard.FindName(controlName));

                        }

                        break;
                }
            }
        }

        private void CreateStoryBoard(DependencyObject target)
        {
            var nextStory = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            var animation = new ColorAnimation
            { 
                To = Colors.White,
                Duration = new Duration(new TimeSpan(0, 0, 1))
            };

            nextStory.Children.Add(animation);

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, "(Path.Fill).(SolidColorBrush.Color)");

            nextStory.Begin();


            _flashers.Add(nextStory);
        }

        private void StopFlashers()
        {
            foreach (var flasher in _flashers)
            {
                flasher.Stop();
            }

            _flashers.Clear();
        }

        private void SetImage(string fileName)
        {
            var dartImage = new BitmapImage(new Uri($"ms-appx:///Assets/{fileName}", UriKind.Absolute));
            Dart1.Source = dartImage;
            Dart2.Source = dartImage;
            Dart3.Source = dartImage;
        }

        public BigBoard()
        {
            InitializeComponent();

            RegisterMessageBus();
        }

        private void RegisterMessageBus()
        {
            MessageBus.Current.Listen<Visibility>().Subscribe(SetDarts);

            MessageBus.Current.Listen<Score>().Subscribe(ThrowDartForNumber);

            MessageBus.Current.Listen<bool>().Subscribe(ResetBoardZoom);
        }

        private void ResetBoardZoom(bool reset)
        {
            ScrollViewer.ChangeView(null, null, 1.0f);
        }

        private void ThrowDartForNumber(Score score)
        {
            if (!Enabled || !score.IsSimpleBoard)
            {
                return;
            }

            if (score.ScoreType == ScoreType.Single)
            {
                // Get path that contains the score (Outer ring for singles)
                var path = score.Value == 0 ? FindName("Single00") as Path : FindName($"Single{score.Value:00}") as Path;

                SetScorePosition(score, path);

                SetDart(score.Position, true, true);
            }

            if (score.ScoreType == ScoreType.Double)
            {
                // Get path that contains the score (Outer ring for singles)
                var path = FindName($"Double{score.NumberHit:00}") as Path;

                SetScorePosition(score, path);

                SetDart(score.Position, true);
            }

            if (score.ScoreType == ScoreType.Treble)
            {
                // Get path that contains the score (Outer ring for singles)
                var path = FindName($"Triple{score.NumberHit:00}") as Path;

                SetScorePosition(score, path);

                SetDart(score.Position, true);
            }

            if (score.ScoreType == ScoreType.Bull)
            {
                // Get path that contains the score (Outer ring for singles)
                var path = FindName("Bull") as Path;

                SetScorePosition(score, path);

                SetDart(score.Position, true);
            }

            if (score.ScoreType == ScoreType.OuterBull)
            {
                // Get path that contains the score (Outer ring for singles)
                var path = FindName("OuterBull") as Path;

                SetScorePosition(score, path);

                SetDart(score.Position, true);
            }

            DartScore = score;
        }

        private void SetScorePosition(Score score, Path path)
        {
            var rectangle = path.GetBoundingRect(dartboard);

            var centrePos = new Point
            {
                X = rectangle.X + (rectangle.Width / 2),
                Y = rectangle.Y + (rectangle.Height / 2),
            };

            score.Position = centrePos;
        }

        private void SetDarts(Visibility visibility)
        {
            Dart1.Visibility = visibility;
            Dart2.Visibility = visibility;
            Dart3.Visibility = visibility;
        }

        private void DartboardOnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var boardPartClicked = e.OriginalSource as Path;
            if (Enabled && boardPartClicked != null)
            {
                var score = new Score { Position = e.GetPosition(this) };
                SetDart(score.Position);

                ClickBoardHelper(boardPartClicked, score);

                DartScore = score;
            }
        }

        private void Dartboard_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var boardPartClicked = e.OriginalSource as Path;
            if (Enabled && boardPartClicked != null)
            {
                var score = new Score { Position = e.GetPosition(this) };
                SetDart(score.Position);

                ClickBoardHelper(boardPartClicked, score);

                DartScore = score;
            }

            e.Handled = true;
        }

        private static void ClickBoardHelper(Path boardPartClicked, Score score)
        {
            if (!string.IsNullOrEmpty(boardPartClicked.Name))
            {
                score.HiLo = HiLo.None;

                if (boardPartClicked.Name == "Bull")
                {
                    score.ScoreType = ScoreType.Bull;
                    score.Value = 50;
                    score.NumberHit = 50;
                }

                if (boardPartClicked.Name == "OuterBull" || boardPartClicked.Name == "OuterBull1")
                {
                    score.ScoreType = ScoreType.OuterBull;
                    score.Value = 25;
                    score.NumberHit = 25;
                }

                if (boardPartClicked.Name.Contains("Single"))
                {
                    score.ScoreType = ScoreType.Single;
                    score.NumberHit = Convert.ToInt32(Regex.Match(boardPartClicked.Name, @"\d+").Value);
                    score.Value = score.NumberHit;
                    score.HiLo = boardPartClicked.Name.EndsWith("L") ? HiLo.Hi : HiLo.Lo;
                }

                if (boardPartClicked.Name.Contains("Double"))
                {
                    score.ScoreType = ScoreType.Double;
                    score.NumberHit = Convert.ToInt32(Regex.Match(boardPartClicked.Name, @"\d+").Value);
                    score.Value = score.NumberHit * 2;
                }

                if (boardPartClicked.Name.Contains("Triple"))
                {
                    score.ScoreType = ScoreType.Treble;
                    score.NumberHit = Convert.ToInt32(Regex.Match(boardPartClicked.Name, @"\d+").Value);
                    score.Value = score.NumberHit * 3;
                }
            }
        }
        
        private void SetDart(Point point, bool adjustX = false, bool adjustY = false)
        {
            var darts = new[] { Dart1, Dart2, Dart3 };

            foreach (var dart in darts)
            {
                if (dart.Visibility == Visibility.Collapsed)
                {
                    MainLocator.CommentaryPlayer.Play(new Commentary
                    {
                        PlaySounds = PlaySound,
                        SoundFiles = new[] { "dart.wav" }
                    });

                    var deltaX = 0;

                    if (adjustX && dart.Name == "Dart2")
                    {
                        deltaX = 10;
                    }

                    if (adjustX && dart.Name == "Dart3")
                    {
                        deltaX = 15;
                    }

                    var deltaY = 0;

                    if (adjustY && dart.Name == "Dart2")
                    {
                        deltaY = 10;
                    }

                    if (adjustY && dart.Name == "Dart3")
                    {
                        deltaY = 15;
                    }

                    var x = point.X / ScrollViewer.ZoomFactor;
                    var y = point.Y / ScrollViewer.ZoomFactor;

                    deltaX = (int)(deltaX / ScrollViewer.ZoomFactor);
                    deltaY = (int)(deltaY / ScrollViewer.ZoomFactor);

                    var dartWidth = dart.Width / 2;
                    var dartHeight = dart.Height / 2;

                    var hOffset = ScrollViewer.HorizontalOffset / ScrollViewer.ZoomFactor;
                    var vOffset = ScrollViewer.VerticalOffset / ScrollViewer.ZoomFactor;

                    Canvas.SetLeft(dart, x + hOffset - dartWidth + deltaX);
                    Canvas.SetTop(dart, y + vOffset - dartHeight + deltaY);

                    dart.Visibility = Visibility.Visible;
                    Canvas.SetZIndex(dart, 100);

                    return;
                }
            }
        }
    }
}
