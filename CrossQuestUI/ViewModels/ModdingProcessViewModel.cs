using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CrossQuestUI.Services;

namespace CrossQuestUI.ViewModels
{
    public class ModdingProcessViewModel : PageViewModelBase
    {
        public string Title => "Modding Game";
        public string Message => "Currently modding the game!";

        public ObservableCollection<string> LogMessages { get; set; } = new ObservableCollection<string>(new List<string>());
        

        private readonly IModdingService _moddingService;

        private readonly IStreamLogger _logger;
        
        public ModdingProcessViewModel(IModdingService moddingService, IStreamLogger moddingLogger)
        {
            _moddingService = moddingService;
            _logger = moddingLogger;
        }
        

        public void OnLoad()
        {
            _logger.OnMessage += (sender, args) =>
            {
                var a = args as StreamLogger.StreamLoggerEventArgs;
                LogMessages.Add(a.Message);
            };
            
            // Step 0
            Dispatcher.UIThread.InvokeAsync(_moddingService.PrepareStep);
            
            /*// Step 1
            _moddingService.SetupUnityStep();

            // Step 2
            _moddingService.SetupModsStep();

            // Step 3
            if (_moddingService.CompileProjectStep())
            {
                // Step 4
                _moddingService.PatchApkStep();

                // Step 5
                _moddingService.InstallApkStep();
                return;
            }

            _logger.Add("Failed to compile project");*/
        }

        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => false;
            protected set => throw new NotSupportedException();
        }
    }
}