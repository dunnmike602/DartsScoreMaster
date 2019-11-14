using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DartsScoreMaster.Common;
using DartsScoreMaster.Repositories.Serialization;

namespace DartsScoreMaster
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (Debugger.IsAttached)
            {
                //    this.DebugSettings.EnableFrameRateCounter = true;
                DebugSettings.BindingFailed += DebugSettings_BindingFailed;
                DebugSettings.IsBindingTracingEnabled = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(Views.Hub), e.Arguments);


                NavigationService = new NavigationService(rootFrame);
            }

            UnhandledException += AppUnhandledException;

            // Ensure the current window is active
            Window.Current.Activate();

            Settings.Settings.Initialise();

            await PerformHouseKeeping();
        }

        private async Task PerformHouseKeeping()
        {
            // Peform some housekeeping
            await DartsDataSerializerHelper.PruneBackupDirectories();

            await DartsDataSerializerHelper.PruneErrors();
        }

        private void AppUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SaveAppdata(e);
            e.Handled = false;
        }

        private void SaveAppdata(UnhandledExceptionEventArgs e)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var fileName = DartsDataSerializerHelper.GetErrorFileName(DateTime.Today.ToString(DartsDataSerializerHelper.DateFormat));
                var tFile = folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists).AsTask();
                tFile.Wait();
                var file = tFile.Result;
                var errMessage = "This error occurred on:" + AddNewLine(1) + DateTime.UtcNow + AddNewLine(2);
                errMessage += e.Exception + AddNewLine(1);
                errMessage += "Version:" + Package.Current.Id.Version.Build +"." + Package.Current.Id.Version.Major
                              +"." + Package.Current.Id.Version.Minor + "." + Package.Current.Id.Version.Revision+ AddNewLine(1);
                errMessage += "Architecture:" + Package.Current.Id.Architecture + AddNewLine(1);
                errMessage += "Dev Mode:" + Package.Current.IsDevelopmentMode + AddNewLine(2);

                var appendTask = FileIO.AppendTextAsync(file, errMessage).AsTask();
                appendTask.Wait();
            }
            catch (Exception)
            {
            }
        }

        private string AddNewLine(int number)
        {
            return string.Concat(Enumerable.Repeat(Environment.NewLine, number));
        }

        public static INavigationService NavigationService { get; set; }

        private void DebugSettings_BindingFailed(object sender, BindingFailedEventArgs e)
        {

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private  void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}
