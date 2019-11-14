using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using DartsScoreMaster.Model;
using DartsScoreMaster.ViewModels.Interfaces;
using DartsScoreMaster.Views;
using ReactiveUI;
using GalaSoft.MvvmLight;

namespace DartsScoreMaster.ViewModels
{
    [DataContract]
    public abstract class BaseViewModel : ObservableObject, IBaseViewModel
    {
        private bool _isDirty;
        private Guid _parentUniqueKey;
        private ReactiveList<Game> _games;
        private ReactiveList<SimpleTuple<int, string>> _statistics;
        private Game _selectedGame;
        private SimpleTuple<int, string> _selectedStatistic;

        protected IDisposable CommandStream { get; set; }

        public ReactiveCommand<object> BackCommand { get; private set; }

        public Thickness PlayerInputThickness
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(20, 5, 20, 5);
                }

                return new Thickness(20, 10, 20, 10);
            }
        }

        public Thickness PlayerCaptionThickness
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(20, 4, 0, 0);
                }

                return new Thickness(20, 12, 0, 0);
            }
        }

        public Thickness BustMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(0.0, 5.0, 5.0, 0.0);
                }

                return new Thickness(0.0, 25.0, 25.0, 0.0);
            }
        }

        public Thickness StatisticsViewButtonMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(0.0, 150.0, -35, 0.0);
                }

                return new Thickness(0.0, 150.0, -65.0, 0.0);
            }
        }


        public Thickness DartboardMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(45);
                }

                return new Thickness(30);
            }
        }

        public Thickness DartsDetailTextMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(2, 2.0, 5.0, 2.0);
                }

                return new Thickness(5, 5, 15.0, 5.0);
            }
        }

        public Thickness DartsDetailScrollViewerMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(0.0, 5, 20.0, 0.0);
                }

                return new Thickness(0.0, 10.0, 45, 0.0);
            }
        }

        public Thickness PlayerTopBarMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(20, 20, 475, 0.0);
                }

                return new Thickness(20, 20, 550, 0.0);
            }
        }

        public double AcceptRejectImageWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 100;
                }

                return 100;
            }
        }

        public double ShowDetailsButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 140;
                }

                return 180;
            }
        }

        public double ShowDetailsButtonHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 40;
                }

                return 60;
            }
        }

        public double WarningFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14;
                }

                return 20;
            }
        }
        
        public double BigScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 50;
                }

                return 60;
            }
        }

        public double CheckoutHintsFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 20;
            }
        }

        public double SettingComboFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 11;
                }

                return 16;
            }
        }

        public double DartsDetailFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 18;
                }

                return 25;
            }
        }

        public Thickness DartsScoreMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(0.0, 0.0, 10.0, 10.0);
                }

                return new Thickness(0.0, 0.0, 30.0, 10.0);
            }
        }

        public Thickness DartsScoreGridMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(8, 5, 0.0, 0.0);
                }

                return new Thickness(24, 15.0, 0.0, 0.0);
            }
        }

        public Thickness AddPlayerImageMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(10, 10, 10, 10);
                }

                return new Thickness(25, 25, 25, 25);
            }
        }

        public Thickness NoScoreMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(-320, 0, 0, 0);
                }

                return new Thickness(-420, 0, 0, 0);
            }
        }

        public Thickness GameShotMargin
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new Thickness(0.0, 5.0, 5.0, 0.0);
                }

                return new Thickness(0.0, 25.0, 25.0, 0.0);
            }
        }

        public GridLength TopRowHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new GridLength(1.2, GridUnitType.Star);

                }

                return new GridLength(1, GridUnitType.Star);
            }
        }

        public GridLength PlayerPageColumn1Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return new GridLength(2, GridUnitType.Star);

                }

                return new GridLength(2.5, GridUnitType.Star);
            }
        }

        public double PopupWidth
        {
            get { return Window.Current.Bounds.Width / 1.9; }
        }

        public double PopupHeight
        {
            get
            {
                return Window.Current.Bounds.Width <= 1372
                    ? Window.Current.Bounds.Height / 1.4
                    : Window.Current.Bounds.Height / 1.6;
            }
        }

        public double UploadButtonHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1650 && Window.Current.Bounds.Width > 1372)
                {
                    return 50;
                }

                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 30;
                }

                return 60;
            }
        }

        public double PlayerButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1650 && Window.Current.Bounds.Width > 1372)
                {
                    return 110;
                }

                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 80;
                }

                return 140;
            }
        }

        public double UploadButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 90;
                }

                return 140;
            }
        }

        public double MarkerWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 150;
                }

                return 220;
            }
        }

        public double StandardImageSize
        {
            get { return Window.Current.Bounds.Width / 5; }
        }

        public double ChartPopupWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return Window.Current.Bounds.Height * 0.8;
                }

                return Window.Current.Bounds.Width * 0.65;


            }
        }

        public double ChartPopupHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return Window.Current.Bounds.Height * 0.8;
                }

                return Window.Current.Bounds.Height * 0.65;
            }
        }

        public double StartGameFont
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 15;
                }

                return 20;
            }
        }


        public double ThrowFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 20;
                }

                return 20;
            }
        }

        public double TotalThrowFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 18;
                }

                return 20;
            }
        }

        public double HelpTextFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 18;
                }

                return 25;
            }
        }

        public double StatisticsViewColWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 100;
                }

                return 145;
            }
        }

        public double StatisticsCaptionFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 12;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 10;
                }

                return 20;
            }
        }

        public double CaptionFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1650 && Window.Current.Bounds.Width > 1372)
                {
                    return 20;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 12;
                }

                return 24;
            }
        }

        public double GraphFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14;
                }

                return 18;
            }
        }

        public double DetailFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 10;
                }

                return 20;
            }
        }

        public double HelpButtonFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 22;
            }
        }

        public double TitleFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 19;
                }

                return 30;
            }
        }

        public double DartImageWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 18;
                }

                return 32;
            }
        }

        public double DartImageWidthLarge
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 40;
                }

                return 80;
            }
        }
        public double StandardDartImageHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 25;
                }

                return 36;
            }
        }

        public double StandardDartImageWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 25;
                }

                return 40;
            }
        }

        public double ScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 20;
            }
        }

        public double WinFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 22;
                }

                return 30;
            }
        }

        public double SmallerButtonFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 12;
                }

                return 18;
            }
        }

        public double DetailItemFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 20;
            }
        }

        public double HubTextFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 20;
                }

                return 28;
            }
        }

        public double FlipButtonHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 75;
                }

                return 110;
            }
        }

        public double FlipButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 75;
                }

                return 110;
            }
        }

        public double HelpButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 120;
                }

                return 190;
            }
        }

        public double NewButtonHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 40;
                }

                return 45;
            }
        }


        public double NewButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 115;
                }

                return 145;
            }
        }

        public double UndoButtonWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 80;
                }

                return 100;
            }
        }

        public double UndoButtonHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 35;
                }

                return 45;
            }
        }

        public double ChartLabelFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14;
                }

                return 22;
            }
        }

        public double SmallButtonFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 20;
            }
        }

        public double ButtonFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 32;
                }

                return 50;
            }
        }

        public double BoardButtonFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 24;
                }

                return 30;
            }
        }

        public double ButtonSmallFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16;
                }

                return 24;
            }
        }

        public double CricketImageSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14;
                }

                return 20;
            }
        }

        public double CricketNickNameFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16D;
                }

                return 20D;
            }
        }

        public double CricketFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16D;
                }

                return 23D;
            }
        }

        public double CricketXFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 18D;
                }

                return 25D;
            }
        }

        public double CricketScoreFontSize
        {
            get { return Window.Current.Bounds.Width <= 1372 ? 17D : 24D; }
        }

        public double ScoreIndWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 40;
                }

                return 50;
            }
        }

        public double TotalScoreIndWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 50;
                }

                return 65;
            }
        }

        public double DartsImageSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 60;
                }

                return 120;
            }
        }

        public double SetScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14D;
                }

                return 20D;
            }
        }

        public double LegScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14D;
                }

                return 20D;
            }
        }

        public double CricketSetScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 12D;
                }

                return 20D;
            }
        }

        public double CricketLegScoreFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 12D;
                }

                return 20D;
            }
        }

        public double DartFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14D;
                }

                return 24D;
            }
        }


        public double ScoreNickNameFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 16D;
                }

                return 22D;
            }
        }

        public double NameFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 20D;
                }

                return 24D;
            }
        }

        public double GameLabelSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 22D;
                }

                return 24D;
            }
        }

        public double PlayerNameFontSize
        {
            get
            {

                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 14;
                }

                return 18;
            }
        }

        public double HeaderFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 14;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 8;
                }

                return 22;
            }
        }

        public double NickNameFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 14;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 10;
                }

                return 22;
            }
        }

        public double PlayerCol1Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 100;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 75;
                }

                return 140;
            }
        }

        public double PlayerCol2Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 125;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 90;
                }

                return 200;
            }
        }

        public double PlayerCol3Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 110;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 80;
                }

                return 160;
            }
        }


        public double PlayerCol4Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 80;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 50;
                }

                return 160;
            }
        }


        public double PlayerCol5Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 80;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 50;
                }

                return 160;
            }
        }


        public double PlayerCol6Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 80;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 60;
                }

                return 160;
            }
        }


        public double PlayerCol7Width
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1373 && Window.Current.Bounds.Width > 1029)
                {
                    return 110;
                }


                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 100;
                }

                return 175;
            }
        }

        public double AcceptScreenHorizOffset
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return -200;
                }

                return -150;
            }
        }

        public double AcceptScreenWidth
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 400;
                }

                return 300;
            }
        }
        
        public double AcceptButtonSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 128;
                }

                return 64;
            }
        }

        public double AcceptFontSize
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 24;
                }

                return 20;
            }
        }

        public double AcceptScreenVertOffset
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return -110;
                }

                return -100;
            }
        }

        public double AcceptScreenHeight
        {
            get
            {
                if (Window.Current.Bounds.Width <= 1372)
                {
                    return 220;
                }

                return 200;
            }
        }

        protected BaseViewModel()
        {
            InitialiseCommands();

            InitialiseStandingData();

#if DEBUG

#else
            CurrentApp.LicenseInformation.LicenseChanged += LicenseInformationLicenseChanged;
#endif
        }

        private async void LicenseInformationLicenseChanged()
        {
            var licenceInfo = await LicenseInformationStore.Get();

            if (licenceInfo.IsTrial && !licenceInfo.IsActive)
            {
                var messageDialog =
                    new MessageDialog("Your trial has expired, you will be returned to the main screen where you can purchase a full licence.",
                        "Darts Score Master");

                await messageDialog.ShowAsync();

                BackCommandHandler(null);
            }
        }

        private void InitialiseStandingData()
        {
            Games = new ReactiveList<Game>
            {
                new Game
                {Id = GameType.T501, Name = "501", StartingScore = 501, ClassId = "x01", ShowCheckOutHints = true, PlayerTemplateName = "Standard"},
                new Game
                {Id = GameType.T401, Name = "401", StartingScore = 401, ClassId = "x01", ShowCheckOutHints = true, PlayerTemplateName = "Standard"},
                new Game
                {Id = GameType.T301, Name = "301", StartingScore = 301, ClassId = "x01", ShowCheckOutHints = true, PlayerTemplateName = "Standard"},
                new Game
                {Id = GameType.T201, Name = "201", StartingScore = 201, ClassId = "x01", ShowCheckOutHints = true, PlayerTemplateName = "Standard"},
                new Game
                {Id = GameType.T101, Name = "101", StartingScore = 101, ClassId = "x01", ShowCheckOutHints = true, PlayerTemplateName = "Standard"},
                new Game {Id = GameType.Cricket, Name = "Cricket", ClassId = "cricket", ShowCheckOutHints = false, PlayerTemplateName = "Cricket"},
            };

            SelectedGame = Games.First(m => m.StartingScore == 501);

            StatisticsList = new ReactiveList<SimpleTuple<int, string>>
            {
                new SimpleTuple<int, string> {Name = 0, Value = "1 Dart Average"},
                new SimpleTuple<int, string> {Name = 1, Value = "3 Dart Average"},
                new SimpleTuple<int, string> {Name = 2, Value = "Checkout %"},
                new SimpleTuple<int, string> {Name = 3, Value = "High 3 Dart Score"},
                new SimpleTuple<int, string> {Name = 4, Value = "Hi Checkout"},
                new SimpleTuple<int, string> {Name = 5, Value = "Darts/Won Leg"},
                new SimpleTuple<int, string> {Name = 6, Value = "Matches Won %"},
                new SimpleTuple<int, string> {Name = 7, Value = "180's Thrown"},
                new SimpleTuple<int, string> {Name = 8, Value = "140's Thrown"},
                new SimpleTuple<int, string> {Name = 9, Value = "100's Thrown"},
                new SimpleTuple<int, string> {Name = 10, Value = "9 Darters"},
                new SimpleTuple<int, string> {Name = 11, Value = "12 Darters"},
                new SimpleTuple<int, string> {Name = 12, Value = "Best Game"},
            };

            SelectedStatistic = StatisticsList.First(m => m.Name == 0);
        }

        private void InitialiseCommands()
        {
            BackCommand = ReactiveCommand.Create();
            BackCommand.Subscribe(BackCommandHandler);

            NavigateToScorer = ReactiveCommand.Create();
            NavigateToScorer.Subscribe(NavigateToScorerCommandHandler);

            NavigateToPlayers = ReactiveCommand.Create();
            NavigateToPlayers.Subscribe(NavigateToPlayerCommandHandler);

        }

        protected async void NavigateToPlayerCommandHandler(object arg)
        {
            var licenceInformation = await GetLicenceInformation();

            if (licenceInformation.IsTrial && !licenceInformation.IsActive)
            {
                return;
            }

            App.NavigationService.Navigate(typeof(Players));
        }

        protected async void NavigateToScorerCommandHandler(object arg)
        {
            var licenceInformation = await GetLicenceInformation();

            if (licenceInformation.IsTrial && !licenceInformation.IsActive)
            {
                return;
            }

            App.NavigationService.Navigate(typeof(MainPage));
        }

        protected void BackCommandHandler(object arg)
        {
            App.NavigationService.Navigate(typeof(Hub));
        }

        [DataMember]
        public bool IsDirty
        {
            get { return _isDirty; }
            set { Set(() => IsDirty, ref _isDirty, value); }
        }

        public Guid ParentUniqueKey
        {
            get { return _parentUniqueKey; }
            set { Set(() => ParentUniqueKey, ref _parentUniqueKey, value); }
        }

        public SimpleTuple<int, string> SelectedStatistic
        {
            get { return _selectedStatistic; }
            set { Set(() => SelectedStatistic, ref _selectedStatistic, value); }
        }

        public Game SelectedGame
        {
            get { return _selectedGame; }
            set { Set(() => SelectedGame, ref _selectedGame, value); }
        }

        public ReactiveList<SimpleTuple<int, string>> StatisticsList
        {
            get { return _statistics; }
            set { Set(() => StatisticsList, ref _statistics, value); }
        }

        public ReactiveList<Game> Games
        {
            get { return _games; }
            set { Set(() => Games, ref _games, value); }
        }

        public ReactiveCommand<object> NavigateToPlayers { get; private set; }
        public ReactiveCommand<object> NavigateToScorer { get; private set; }

        protected async Task<LicenseInformation> GetLicenceInformation()
        {
            return await LicenseInformationStore.Get();
        }

        protected IEnumerable<DateTime> GetDays(int number)
        {
            var now = DateTime.Now.Date;

            var days = Enumerable.Range(-number, number + 1)
                .Select(x => now.AddDays(x));

            return days;
        }

        protected IEnumerable<int> GetYears(int number)
        {
            var now = DateTime.Now;
            now = now.Date.AddDays(1 - now.Day);

            return Enumerable.Range(-number, number + 1)
                .Select(x => now.AddYears(x).Year);
        }

        protected IEnumerable<Tuple<int, int>> GetMonths(int number)
        {
            var now = DateTime.Now;
            now = now.Date.AddDays(1 - now.Day);
            return Enumerable.Range(-number, number + 1)
                .Select(x => new Tuple<int, int>(
                    now.AddMonths(x).Year,
                    now.AddMonths(x).Month
                    ));
        }

        public void UpdateSizes()
        {
            foreach (var property in typeof(BaseViewModel).GetRuntimeProperties())
            {
                RaisePropertyChanged(property.Name);
            }
        }

    }
}