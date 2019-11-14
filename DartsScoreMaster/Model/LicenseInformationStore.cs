using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;

namespace DartsScoreMaster.Model
{
    public static class LicenseInformationStore
    {
        public static string PurchaseResult { get; set; }

        private static async Task LoadConfigFile()
        {
            var storageFolder = Package.Current.InstalledLocation;
            var storageFileResult = await storageFolder.GetFileAsync(@"trial-mode.xml");

            await CurrentAppSimulator.ReloadSimulatorAsync(storageFileResult);
        }

        public static async Task Buy()
        {
            var licenseInformation = await Get();

            if (licenseInformation.IsTrial)
            {
                try
                {
                //     await CurrentAppSimulator.RequestAppPurchaseAsync(false);

                    await CurrentApp.RequestAppPurchaseAsync(false);

                   if (!licenseInformation.IsTrial && licenseInformation.IsActive)
                    {
                        PurchaseResult = "You successfully upgraded your app to the fully-licensed version.";
                    }
                    else
                    {
                        PurchaseResult = "The purchase could not be completed at this time.";
                    }
                }
                catch (Exception)
                {
                    PurchaseResult = "The purchase could not be completed at this time.";
                }
            }
            else
            {
                PurchaseResult = "You already bought this app and have a fully-licensed version.";
            }
        }

        public static async Task<LicenseInformation> Get()
        {

            await LoadConfigFile();

         //   return CurrentAppSimulator.LicenseInformation;

           return CurrentApp.LicenseInformation;
        }
    }
}