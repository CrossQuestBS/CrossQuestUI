using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossQuestUI.Services;

namespace CrossQuestUI.ViewModels
{
    public partial class ModdingProcessViewModel(IModdingService moddingService, IStreamLogger logger)
        : PageViewModelBase
    {
        public string Title => "Modding Game";
        public string Message => "Currently modding the game!";

        [ObservableProperty]
        private bool _isModding;

        public ObservableCollection<string> LogMessages { get; set; } = new ObservableCollection<string>(new List<string>());

        public async Task OnLoad()
        {
            IsModding = true;
            logger.OnMessage += (sender, args) =>
            {
                if (args is not StreamLogger.StreamLoggerEventArgs arg)
                    return;
                
                LogMessages.Add(arg.Message);
            };
            
            // Step 0
            await moddingService.PrepareStep();
            
            // Step 1
            await moddingService.SetupUnityStep();
            
            // Step 2
            moddingService.SetupModsStep();

            // Step 3
            if (await moddingService.CompileProjectStep())
            {
                // Step 4
                await moddingService.PatchApkStep();

                // Step 5
                await moddingService.InstallApkStep();
                IsModding = false;
                return;
            }
            LogMessages.Add("Failed to compile project");
            IsModding = false;
        }

        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigateNext
        {
            get => !IsModding;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
    }
}