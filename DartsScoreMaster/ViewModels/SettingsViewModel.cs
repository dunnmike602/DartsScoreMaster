using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using DartsScoreMaster.Model;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Repositories.Serialization;
using DartsScoreMaster.Services.Interfaces;
using ReactiveUI;

namespace DartsScoreMaster.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ReactiveCommand<object> RestoreCommand { get; private set; }
        public ReactiveCommand<object> ExportCommand { get; private set; }
        public ReactiveCommand<object> LoadSoundsCommand { get; private set; }
        public ReactiveCommand<object> ResetSoundsCommand { get; private set; }
        public ReactiveCommand<object> ClearPlayersCommand { get; private set; }
        public ReactiveCommand<object> EmailCommand { get; private set; }
        
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly IDialogService _dialogService;
        private GameConfiguration _configurationData;
        private bool _playSounds;
        private bool _showCheckoutHints;
        private bool _isBusy;
        private bool _errorsDetected;
        private List<string> _backups;
        private string _selectedBackup;
        private bool _isRestoreAvailable;

        public GameConfiguration ConfigurationData
        {
            get { return _configurationData; }
            set { Set(() => ConfigurationData, ref _configurationData, value); }
        }
        
        public List<string> Backups
        {
            get { return _backups; }
            set { Set(() => Backups, ref _backups, value); }
        }

        public string SelectedBackup
        {
            get { return _selectedBackup; }
            set { Set(() => SelectedBackup, ref _selectedBackup, value); }
        }

        public bool PlaySounds
        {
            get { return _playSounds; }
            set { Set(() => PlaySounds, ref _playSounds, value); }
        }

        public bool ShowCheckoutHints
        {
            get { return _showCheckoutHints; }
            set { Set(() => ShowCheckoutHints, ref _showCheckoutHints, value); }
        }
        
        public bool IsRestoreAvailable
        {
            get { return _isRestoreAvailable; }
            set { Set(() => IsRestoreAvailable, ref _isRestoreAvailable, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        public bool ErrorsDetected
        {
            get { return _errorsDetected; }
            set { Set(() => ErrorsDetected, ref _errorsDetected, value); }
        }
        

        public SettingsViewModel(IConfigurationRepository configurationRepository, IStatisticsRepository statisticsRepository, IDialogService dialogService)
        {
            _configurationRepository = configurationRepository;
            _statisticsRepository = statisticsRepository;
            _dialogService = dialogService;

            CreateCommands();
        }

        public async void Init()
        {
            await LoadConfigData();

            ShowCheckoutHints = ConfigurationData.ShowCheckoutHints;
            PlaySounds = ConfigurationData.PlaySounds;

            CreateSubscriptions();

            CheckForErrors();
        }

        private async void CheckForErrors()
        {
            ErrorsDetected = await DartsDataSerializerHelper.HasErrors();
        }

        private async Task LoadConfigData()
        {
            _configurationData = await _configurationRepository.GetAll();

            Backups = await DartsDataSerializerHelper.GetAllBackups();

            if (Backups.Count > 0)
            {
                SelectedBackup = Backups[0];
            }
        }

        private void CreateCommands()
        {
            RestoreCommand = ReactiveCommand.Create();
            RestoreCommand.Subscribe(arg => RestoreCommandHandler());

            ExportCommand = ReactiveCommand.Create();
            ExportCommand.Subscribe(arg => ExportCommandCommandHandler());

            LoadSoundsCommand = ReactiveCommand.Create();
            LoadSoundsCommand.Subscribe(arg => LoadSoundsCommandHandler());

            ResetSoundsCommand = ReactiveCommand.Create();
            ResetSoundsCommand.Subscribe(arg => ResetSoundsCommandHandler());

            ClearPlayersCommand = ReactiveCommand.Create();
            ClearPlayersCommand.Subscribe(arg => ClearPlayersCommandHandler());

            EmailCommand = ReactiveCommand.Create();
            EmailCommand.Subscribe(async arg => await EmailCommandCommandHandler());
        }

        private async Task EmailCommandCommandHandler()
        {
            var content = await DartsDataSerializerHelper.GetErrorFilesAsText();

            var dataPackage = new DataPackage();
            dataPackage.SetText(content);
            Clipboard.SetContent(dataPackage);

            await Launcher.LaunchUriAsync(new Uri("mailto:enquiries@mldcomputing.com?subject=DartsScoreMaster&body=" + content));

            await DartsDataSerializerHelper.DeleteAllErrorFiles();

            CheckForErrors();

            MessageBus.Current.SendMessage(true, "ErrorsCleared");
        }
        
        private void ClearPlayersCommandHandler()
        {
            _dialogService.ShowMessage("This action is not reversible. Are you sure?.", "Darts Score Master",
                ClearPlayersYesCommandHandler);
        }

        private void ResetSoundsCommandHandler()
        {
            _dialogService.ShowMessage("This action is not reversible. Are you sure?.", "Darts Score Master",
                ResetSoundsYesCommandHandler);
        }

        private async void ClearPlayersYesCommandHandler(IUICommand command)
        {
            var targetFolder = ApplicationData.Current.LocalFolder;

            await targetFolder.DeleteAllConfigFiles(true);
        }

        private async void ResetSoundsYesCommandHandler(IUICommand command)
        {
            var targetFolder = ApplicationData.Current.LocalFolder;

            if (targetFolder != null)
            {
                try
                {
                    IsBusy = true;

                    for (var i = 0; i <= 180; i++)
                    {
                        await DeleteFile(targetFolder, $"s{i}.wav");
                    }

                    for (var i = 0; i <= 170; i++)
                    {
                        await DeleteFile(targetFolder, $"yr{i}.wav");
                    }

                    for (var i = 1; i <= 6; i++)
                    {
                        await DeleteFile(targetFolder, $"tfplayer{i}.wav");
                    }

                    await DeleteFile(targetFolder, "gs.wav");
                    await DeleteFile(targetFolder, "gsm.wav");
                    await DeleteFile(targetFolder, "gss.wav");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async void LoadSoundsCommandHandler()
        {
            var picker = new FolderPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            picker.FileTypeFilter.Add(".wav");

            var folder = await picker.PickSingleFolderAsync();

            var targetFolder = ApplicationData.Current.LocalFolder;

            if (folder != null)
            {
                try
                {
                    IsBusy = true;

                    for (var i = 0; i <= 180; i++)
                    {
                        await CopyFile(folder, $"s{i}.wav", targetFolder);
                    }

                    for (var i = 0; i <= 170; i++)
                    {
                        await CopyFile(folder, $"yr{i}.wav", targetFolder);
                    }

                    for (var i = 1; i <= 6; i++)
                    {
                        await CopyFile(folder, $"tfplayer{i}.wav", targetFolder);
                    }

                    await CopyFile(folder, "gs.wav", targetFolder);
                    await CopyFile(folder, "gsm.wav", targetFolder);
                    await CopyFile(folder, "gss.wav", targetFolder);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private static async Task DeleteFile(StorageFolder folder, string name)
        {
            var soundFile = await folder.TryGetItemAsync(name) as StorageFile;
            if (soundFile != null)
            {
                await soundFile.DeleteAsync();
            }
        }

        private static async Task CopyFile(StorageFolder folder, string name, StorageFolder targetFolder)
        {
            var soundFile = await folder.TryGetItemAsync(name) as StorageFile;
            if (soundFile != null)
            {
                await soundFile.CopyAsync(targetFolder, name, NameCollisionOption.ReplaceExisting);
            }
        }

        private async void ExportCommandCommandHandler()
        {
            var downloadData = await _statisticsRepository.GetAllForDownload();

            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            picker.FileTypeChoices.Add("CSV", new List<string> { ".csv" });
            picker.SuggestedFileName = "DartsScoreMasterStats";

            var file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteTextAsync(file, downloadData);

                var status = await CachedFileManager.CompleteUpdatesAsync(file);

                if (status != FileUpdateStatus.Complete)
                {
                    throw new Exception($"File could not be saved, status was {status}.");
                }
            }
        }

        private void RestoreCommandHandler()
        {
            _dialogService.ShowMessage("This action is not reversible. Are you sure?.", "Darts Score Master",
                RestoreDataYesCommandHandler);
        }

        private async void RestoreDataYesCommandHandler(IUICommand command)
        {
            if (string.IsNullOrWhiteSpace(SelectedBackup))
            {
                return;
            }

            var date = DateTime
                .ParseExact(SelectedBackup, DartsDataSerializerHelper.LongDateFormat, CultureInfo.InvariantCulture)
                .ToString(DartsDataSerializerHelper.DateFormat);

            await _configurationRepository.Restore(DartsDataSerializerHelper.GetBackupFolder(date));

           RestartAppHelper();
        }

        private static void RestartAppHelper()
        {
            Application.Current.Exit();
        }

        private void CreateSubscriptions()
        {
            this.WhenAnyValue(x => x.PlaySounds, y => y.ShowCheckoutHints).Skip(1).Subscribe(
                _ => SaveConfigData());

            this.WhenAny(x => x.IsBusy, y => y.SelectedBackup,
                (isBusy, sb) => !isBusy.Value && !string.IsNullOrWhiteSpace(sb.Value)).Subscribe(x=> IsRestoreAvailable = x);
        }

        private void SaveConfigData()
        {
            ConfigurationData.PlaySounds = PlaySounds;
            ConfigurationData.ShowCheckoutHints = ShowCheckoutHints;

            _configurationRepository.Add(ConfigurationData);
            _configurationRepository.Save();

            MessageBus.Current.SendMessage(ConfigurationData);
        }


    }
}