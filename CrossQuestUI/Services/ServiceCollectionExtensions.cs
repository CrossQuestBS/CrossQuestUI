using Avalonia.Platform.Storage;
using CrossQuestUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CrossQuestUI.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IModdingService, ModdingService>();
            collection.AddSingleton<IStreamLogger, StreamLogger>();
            collection.AddSingleton<IApkSigner, ApkSigner>();
            collection.AddSingleton<IDownloader, Downloader>();
            collection.AddSingleton<IApkPatcher, ApkPatcher>();
            collection.AddSingleton<IApkTool, ApkTool>();
            collection.AddSingleton<IAdbClient, AdbClient>();
            collection.AddSingleton<IGithubDownloader, GithubDownloader>();
            collection.AddSingleton<IAndroidService, AndroidService>();
            collection.AddSingleton<IFilesService, FilesService>();
            collection.AddSingleton<IUnityEditor, UnityEditor>();

            collection.AddTransient<MainWindowViewModel>();
            collection.AddTransient<IntroductionViewModel>();
            collection.AddTransient<ConfigSetupViewModel>();
            collection.AddTransient<VerificationViewModel>();
            collection.AddTransient<BaseFileDownloadViewModel>();
            collection.AddTransient<InitialModSetupViewModel>();
            collection.AddTransient<ModdingProcessViewModel>();
            collection.AddTransient<DashboardViewModel>();
        }
    }
}