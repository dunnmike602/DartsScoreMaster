using ReactiveUI;
using System;

namespace DartsScoreMaster.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        private bool _showTab0;
        private bool _showTab1;
        private bool _showTab2;
        private bool _showTab3;
        private bool _showTab4;
        private bool _showTab5;

        public bool ShowTab0
        {
            get { return _showTab0; }
            set
            {
                Set(() => ShowTab0, ref _showTab0, value);
            }
        }

        public bool ShowTab1
        {
            get { return _showTab1; }
            set
            {
                Set(() => ShowTab1, ref _showTab1, value);
            }
        }


        public bool ShowTab2
        {
            get { return _showTab2; }
            set
            {
                Set(() => ShowTab2, ref _showTab2, value);
            }
        }


        public bool ShowTab3
        {
            get { return _showTab3; }
            set
            {
                Set(() => ShowTab3, ref _showTab3, value);
            }
        }


        public bool ShowTab4
        {
            get { return _showTab4; }
            set
            {
                Set(() => ShowTab4, ref _showTab4, value);
            }
        }


        public bool ShowTab5
        {
            get { return _showTab5; }
            set
            {
                Set(() => ShowTab5, ref _showTab5, value);
            }
        }

        public ReactiveCommand<object> ChangeTabCommand { get; private set; }

        public HelpViewModel()
        {
            InitaliseCommands();
        }

        private void InitaliseCommands()
        {
            ChangeTabCommand = ReactiveCommand.Create();
            ChangeTabCommand.Subscribe(ChangeTabCommandHandler);

            ChangeTabCommandHandler(0);

        }

        private void ChangeTabCommandHandler(object arg)
        {
            var tabId = Convert.ToInt32(arg);

            ShowTab0 = ShowTab1 = ShowTab2 = ShowTab3 = ShowTab4 = ShowTab5 = false;

            switch(tabId)
            {
                case 0:
                    ShowTab0 = true;
                    break;

                case 1:
                    ShowTab1 = true;
                    break;

                case 2:
                    ShowTab2 = true;
                    break;

                case 3:
                    ShowTab3 = true;
                    break;

                case 4:
                    ShowTab4 = true;
                    break;

                case 5:
                    ShowTab5 = true;
                    break;

            }
        }
    }
}