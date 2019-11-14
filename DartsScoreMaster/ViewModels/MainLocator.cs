using System;
using Windows.Media.SpeechSynthesis;
using DartsScoreMaster.Repositories;
using DartsScoreMaster.Repositories.Interfaces;
using DartsScoreMaster.Repositories.Serialization;
using DartsScoreMaster.Repositories.Serialization.Interfaces;
using DartsScoreMaster.Services;
using DartsScoreMaster.Services.Interfaces;
using DartsScoreMaster.Tasks;
using DartsScoreMaster.Tasks.Interfaces;
using DartsScoreMaster.ViewModels.Interfaces;
using Microsoft.Practices.Unity;

namespace DartsScoreMaster.ViewModels
{
    public class MainLocator
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        static MainLocator()
        {
            UnityContainer.RegisterType(typeof(ISerialization<>), typeof(DartsDataSerializer<>),
                new ContainerControlledLifetimeManager());

            UnityContainer.RegisterType<HelpViewModel, HelpViewModel>(
                new ContainerControlledLifetimeManager());

            UnityContainer.RegisterType<IPlayersRepository, PlayersRepository>(
                new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IConfigurationRepository, ConfigurationRepository>(
                new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IStatisticsRepository, StatisticsRepository>(
                new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IImageLoadTask, ImageLoadTask>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<MainPageViewModel, MainPageViewModel>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<HubViewModel, HubViewModel>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<PlayerViewModel, PlayerViewModel>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<SettingsViewModel, SettingsViewModel>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<StatisticsViewModel, StatisticsViewModel>(
                new ContainerControlledLifetimeManager());

            UnityContainer.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<IWavePlayer, WavePlayer>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<SpeechSynthesizer, SpeechSynthesizer>(new ContainerControlledLifetimeManager());
            UnityContainer.RegisterType<ICommentaryPlayer, CommentaryPlayer>(new ContainerControlledLifetimeManager());

            UnityContainer.RegisterType<IDartGame, StraightDarts>("x01");
            UnityContainer.RegisterType<IDartGame, Cricket>("cricket");

            UnityContainer.RegisterType<Func<string, IDartGame>>(
                new InjectionFactory(c =>
                    new Func<string, IDartGame>(name => c.Resolve<IDartGame>(name))));


            UnityContainer.RegisterType<IStatisticsCalculationService, StandardStatisticsCalculationService>("x01");
            UnityContainer.RegisterType<IStatisticsCalculationService, CricketStatisticsCalculationService>("cricket");

            UnityContainer.RegisterType<Func<string, IStatisticsCalculationService>>(
                new InjectionFactory(c =>
                    new Func<string, IStatisticsCalculationService>(
                        name => c.Resolve<IStatisticsCalculationService>(name))));
        }

        public static SettingsViewModel SettingsViewModel => UnityContainer.Resolve<SettingsViewModel>();

        public static HelpViewModel HelpViewModel => UnityContainer.Resolve<HelpViewModel>();

        public StatisticsViewModel StatisticsViewModel => UnityContainer.Resolve<StatisticsViewModel>();

        public static PlayerViewModel PlayerViewModel => UnityContainer.Resolve<PlayerViewModel>();

        public static HubViewModel HubViewModel => UnityContainer.Resolve<HubViewModel>();

        public MainPageViewModel MainPageViewModel => UnityContainer.Resolve<MainPageViewModel>();


        public static ICommentaryPlayer CommentaryPlayer => UnityContainer.Resolve<CommentaryPlayer>();
    }
}