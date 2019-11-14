using DartsScoreMaster.ViewModels.Interfaces;
using ReactiveUI;
using System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace DartsScoreMaster.Views
{
    public sealed partial class StatisticsView
    {
        private bool _shouldShow = true;

        public ReactiveCommand<object> SlideCommand
        {
            get { return (ReactiveCommand<object>)GetValue(SlideCommandProperty); }
            set
            {
                SetValue(SlideCommandProperty, value);
            }
        }

        private void SlideCommandHandler(object parameter)
        {
            Reveal.Stop();
            Hide.Stop();

            if (parameter is bool)
            {
                if (!_shouldShow)
                {
                    Hide.Begin();
                    _shouldShow = true;
                }

                return;
            }

            if (_shouldShow)
            {
                Reveal.Begin();
            }
            else
            {
                Hide.Begin();
            }

            _shouldShow = !_shouldShow;
        }

        public static readonly DependencyProperty SlideCommandProperty =
            DependencyProperty.Register("SlideCommand", typeof(ReactiveCommand<object>), typeof(StatisticsView),
                new PropertyMetadata(null, SlideChangedEventHandler));

        private static void SlideChangedEventHandler(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var box = source as StatisticsView;

            box?.SetUpCommand();
        }

        private void SetUpCommand()
        {
            SlideCommand.Subscribe(SlideCommandHandler);
        }

        public Guid PlayerId
        {
            get { return (Guid)GetValue(PlayerIdProperty); }
            set
            {
                SetValue(PlayerIdProperty, value);
            }
        }

        public static readonly DependencyProperty PlayerIdProperty =
            DependencyProperty.Register("PlayerId", typeof(Guid), typeof(StatisticsView),
                new PropertyMetadata(null, PlayerIdPropertyChanged));

        private readonly GestureRecognizer _recogniser;

        private static void PlayerIdPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var view = source as StatisticsView;

            var baseViewModel = view?.GridRoot.DataContext as IBaseViewModel;

            if (baseViewModel != null && e.NewValue != null)
            {
                baseViewModel.ParentUniqueKey = (Guid)e.NewValue;
            }
        }

        public StatisticsView()
        {
            InitializeComponent();

            _recogniser = new GestureRecognizer { GestureSettings = GestureSettings.ManipulationTranslateX };

            _recogniser.ManipulationCompleted += OnManipulationCompleted;

            FullScreen.PointerMoved += OnPointerMoved;
            FullScreen.PointerCanceled += OnPointerCanceled;
            FullScreen.PointerPressed += OnPointerPressed;
            FullScreen.PointerReleased += OnPointerReleased;
        }

        private void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            Hide.Begin();
            _shouldShow = true;
        }

        private void OnPointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            _recogniser.ProcessDownEvent(args.GetCurrentPoint(ControlRoot));

            FullScreen.CapturePointer(args.Pointer);

            args.Handled = true;
        }

        private void OnPointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            _recogniser.ProcessUpEvent(args.GetCurrentPoint(ControlRoot));
            args.Handled = true;
        }

        private void OnPointerCanceled(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            _recogniser.CompleteGesture();
            args.Handled = true;
        }

        private void OnPointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _recogniser.ProcessMoveEvents(e.GetIntermediatePoints(ControlRoot));

            e.Handled = true;
        }

        private void StatisticsViewOnLoaded(object sender, RoutedEventArgs e)
        {
            StartValue.Value = -ControlRoot.ActualWidth - 50;
            EndValue.Value = -ControlRoot.ActualWidth - 50;
            var transforms = new CompositeTransform { TranslateX = StartValue.Value };
            ControlRoot.RenderTransform = transforms;

            Loaded -= StatisticsViewOnLoaded;
        }
    }
}
