using System;
using Windows.UI.Xaml;

namespace DartsScoreMaster.Controls
{
    [TemplateVisualState(Name = "Front", GroupName = "Normal")]
    [TemplateVisualState(Name = "Back", GroupName = "Normal")]
    [TemplateVisualState(Name = "Reverse", GroupName = "Normal")]
    public sealed partial class Flipper 
    {
        private bool _isInitialising = true;

        public static readonly DependencyProperty StateProperty = DependencyProperty.
            Register("State", typeof(FlipperStates), typeof(Flipper), new PropertyMetadata(FlipperStates.Front, OnPropertyChanged));

        public static readonly DependencyProperty InitialStateProperty = DependencyProperty.
            Register("InitialState", typeof(FlipperStates), typeof(Flipper), new PropertyMetadata(FlipperStates.Front, OnInitStateChanged));

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var flipper = ((Flipper)(sender));

            flipper.UpdateState(e.NewValue.ToString(), e.OldValue.ToString());
        }


        private static void OnInitStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var flipper = ((Flipper)(sender));

            flipper.SetInitialState();
        }

        private void SetInitialState()
        {
            if (InitialState == FlipperStates.Back)
            {
                VisualStateManager.GoToState(this, "Reverse", true);
            }
        }

        private void UpdateState(string newState, string oldState)
        {
            if (string.Compare(newState, oldState, StringComparison.OrdinalIgnoreCase) != 0 && !_isInitialising)
            {
                VisualStateManager.GoToState(this, newState, true);
            }
            else
            {
                _isInitialising = false;
            }
        }

        public FlipperStates State
        {
            get { return (FlipperStates)GetValue(StateProperty); }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        public FlipperStates InitialState
        {
            get { return (FlipperStates)GetValue(InitialStateProperty); }
            set
            {
                SetValue(InitialStateProperty, value);
            }
        }

        public static readonly DependencyProperty FrontContentProperty = DependencyProperty.
         Register("FrontContent", typeof(UIElement), typeof(Flipper), new PropertyMetadata(0.0, null));

        public UIElement FrontContent
        {
            get { return (UIElement)GetValue(FrontContentProperty); }
            set
            {
                SetValue(FrontContentProperty, value);
            }
        }

        public static readonly DependencyProperty BackContentProperty = DependencyProperty.
       Register("BackContent", typeof(UIElement), typeof(Flipper), new PropertyMetadata(0.0, null));
    
        public UIElement BackContent
        {
            get { return (UIElement)GetValue(BackContentProperty); }
            set
            {
                SetValue(BackContentProperty, value);
            }
        }

        public Flipper()
        {
            InitializeComponent();
        }
    }
}
