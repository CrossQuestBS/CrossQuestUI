using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrossQuestUI.Models;
using CrossQuestUI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CrossQuestUI.ViewModels
{
    public partial class ConfigSetupViewModel : PageViewModelBase
    {
        [ObservableProperty]
        private string _editorPath = "";
        
        [ObservableProperty]
        private string _questGame = "";
        
        [ObservableProperty]
        private string _gamePath = "";
        
        [ObservableProperty]
        private string _androidPlayer = "";
        
        [ObservableProperty]
        private string _apkToolPath = "";

        private readonly IFilesService _filesService;

        public ConfigSetupViewModel(IFilesService filesService)
        {
            PropertyChanged += (sender, args) =>
            {
                Update();
            };
            _filesService = filesService;
        }
        

        [ObservableProperty] private bool _hasFilledOut;
        private void Update()
        {
            HasFilledOut = Directory.Exists(GamePath) && File.Exists(QuestGame) && File.Exists(EditorPath) &&
                           Directory.Exists(AndroidPlayer) && File.Exists(ApkToolPath);

            if (HasFilledOut)
            {
                App.Current?.ModdingConfig = new ModdingConfig(EditorPath, QuestGame, GamePath, AndroidPlayer,ApkToolPath,[]);
            }
        }

        [RelayCommand]
        public async Task SetEditorPath()
        {
            var file = await _filesService.OpenFileAsync();

            if (file is not null)
            {
                EditorPath = Uri.UnescapeDataString(file.Path.AbsolutePath);
            }
        }
        
        [RelayCommand]
        public async Task SetAndroidPlayerPath()
        {
            var folder = await _filesService.OpenFolderAsync();

            if (folder is not null)
            {
                AndroidPlayer = Uri.UnescapeDataString(folder.Path.AbsolutePath);
            }
        }
        
        [RelayCommand]
        public async Task SetApkToolPath()
        {
            var file = await _filesService.OpenFileAsync();

            if (file is not null)
            {
                ApkToolPath = Uri.UnescapeDataString(file.Path.AbsolutePath);
            }
        }
        
        [RelayCommand]
        public async Task SetQuestPath()
        {
            var file = await _filesService.OpenFileAsync();

            if (file is not null)
            {
                QuestGame = Uri.UnescapeDataString(file.Path.AbsolutePath);
            }

        }
        
        [RelayCommand]
        public async Task SetGamePath()
        {

            var folder = await _filesService.OpenFolderAsync();

            if (folder is not null)
            {
                GamePath = Uri.UnescapeDataString(folder.Path.AbsolutePath);
            }
        }
        
        
        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        
        public override bool CanNavigateNext
        {
            get => HasFilledOut;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
    }
}