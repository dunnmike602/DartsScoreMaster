using DartsScoreMaster.Controls.Interfaces;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Views;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using DartsScoreMaster.Repositories.Serialization;
using DartsScoreMaster.Services.Interfaces;

namespace DartsScoreMaster.ViewModels
{
    public class HubViewModel : VoiceControlViewModel, INavigable
    {
        private readonly IConfigurationRepository _configurationRepository;
        private string _trialText;
        private bool _inTrialMode;
        private bool _enableSoundSupport;
        private string _microphoneSetupString;
        private GameConfiguration _configurationData;
        private bool _errorsDetected;

        public ReactiveCommand<object> NavigateToHelp { get; private set; }

        public ReactiveCommand<object> BuyNewGameCommand { get; private set; }

        public ReactiveCommand<object> ShowSettingsCommand { get; private set; }

        public string TrialText
        {
            get { return _trialText; }
            set { Set(() => TrialText, ref _trialText, value); }
        }

        public bool InTrialMode
        {
            get { return _inTrialMode; }
            set { Set(() => InTrialMode, ref _inTrialMode, value); }
        }

        public bool ErrorsDetected
        {
            get { return _errorsDetected; }
            set { Set(() => ErrorsDetected, ref _errorsDetected, value); }
        }

        public bool EnableSoundSupport
        {
            get { return _enableSoundSupport; }
            set
            {
                Set(() => EnableSoundSupport, ref _enableSoundSupport, value);
            }
        }

        public string MicrophoneSetupString
        {
            get { return _microphoneSetupString; }
            set { Set(() => MicrophoneSetupString, ref _microphoneSetupString, value); }
        }

   
        public HubViewModel(IConfigurationRepository configurationRepository,
            ICommentaryPlayer commentaryPlayer,
            IDialogService dialogService) : base(commentaryPlayer, dialogService)
        {
            _configurationRepository = configurationRepository;

            InitaliseCommands();

            LoadConfigData();

            RegisterMessageBus();
        }

        private void RegisterMessageBus()
        {
            MessageBus.Current.Listen<bool>("ErrorsCleared").Subscribe(async _ => await SetErrorsDetected());
        }

        private async void LoadConfigData()
        {
            _configurationData = await _configurationRepository.GetAll();

            EnableSoundSupport = _configurationData.EnableSoundRecognition;
            IsSoundSupportEnabled = EnableSoundSupport;
        }

        private void InitaliseCommands()
        {

            NavigateToHelp = ReactiveCommand.Create();
            NavigateToHelp.Subscribe(NavigateToHelpCommandHandler);

            BuyNewGameCommand = ReactiveCommand.Create();
            BuyNewGameCommand.Subscribe(BuyNewGameCommandHandler);

            ShowSettingsCommand = ReactiveCommand.Create();

            ShowSettingsCommand.Subscribe(arg => { });
        }

        private async void BuyNewGameCommandHandler(object arg)
        {
            await LicenseInformationStore.Buy();

            var messageDialog = new MessageDialog(LicenseInformationStore.PurchaseResult,
                "Darts Score Master");

            await messageDialog.ShowAsync();

            Activate(null);
        }

        private void NavigateToHelpCommandHandler(object arg)
        {
            App.NavigationService.Navigate(typeof(HelpView));
        }

        public async void Activate(object parameter)
        {
            var licenceInformation = await GetLicenceInformation();

            if (!licenceInformation.IsTrial)
            {
                InTrialMode = false;
            }
            else
            {
                InTrialMode = true;

                TrialText = licenceInformation.IsActive ? "TRIAL VERSION" : "TRIAL EXPIRED";
            }

            await SetErrorsDetected();
        }

        private async Task SetErrorsDetected()
        {
            ErrorsDetected = await DartsDataSerializerHelper.HasErrors();
        }

        public void Deactivate(object parameter)
        {
            _configurationData.EnableSoundRecognition = IsSoundSupportEnabled;
            _configurationRepository.Add(_configurationData);
            _configurationRepository.Save();
        }
    }
}