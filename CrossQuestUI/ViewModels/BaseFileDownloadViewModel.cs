using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossQuestUI.Services;

namespace CrossQuestUI.ViewModels
{
    public partial class BaseFileDownloadViewModel : PageViewModelBase
    {
        public string Title => "Required files";
        public string Message => "CrossQuest is currently downloading required files";
        
        [ObservableProperty]
        private bool _isDownloading;
        
        public ObservableCollection<string> LogMessages { get; set; } = new(new List<string>());
        private readonly IGithubDownloader _githubDownloader = new GithubDownloader(new Downloader());

        private async Task StartDownloading()
        {
            var downloadDir = Path.Join(App.Current?.ApplicationPath, "Downloads");

            Directory.CreateDirectory(downloadDir);
            
            // TODO: Remove hardcoding!
            LogMessages.Add("Downloading Base Project");
            await _githubDownloader.DownloadArchive("CrossQuestBS", "UnityBaseProject", "1-42-0", Path.Join(downloadDir, "UnityBase.zip"));
            LogMessages.Add("Downloading Mods file");
            await _githubDownloader.DownloadRawFile("CrossQuestBS", "Mods", "1.42.0", "mods.json", Path.Join(downloadDir, "mods.json"));
            LogMessages.Add("Done downloading all files!");
            IsDownloading = false;
        }

        public void OnLoad()
        {
            IsDownloading = true;
            Dispatcher.UIThread.InvokeAsync(StartDownloading);
        } 
         
        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        
        public override bool CanNavigateNext
        {
            get => !IsDownloading;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => !IsDownloading;
            protected set => throw new NotSupportedException();
        }
    }
}