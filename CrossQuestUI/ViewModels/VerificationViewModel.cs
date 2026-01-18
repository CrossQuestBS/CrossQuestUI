using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CrossQuestUI.Models;
using CrossQuestUI.Services;

namespace CrossQuestUI.ViewModels
{
    public partial class VerificationViewModel : PageViewModelBase
    {
        public string Title => "Verify requirements";
        public string Message => "CrossQuest is verifying everything is correctly set up";


        [ObservableProperty]
        private bool _isVerified = false;

        private readonly IAndroidService _androidService;
        private readonly IUnityEditor _unityEditor;
        
        public ObservableCollection<VerificationItem> Verifications { get; set; } = new(new List<VerificationItem>());

        public VerificationViewModel(IAndroidService androidService, IUnityEditor unityEditor)
        {
            _androidService = androidService;
            _unityEditor = unityEditor;
        }
        
        public void OnUnload()
        {
            Verifications = new(new List<VerificationItem>());
        }
        
        public async Task OnLoad()
        {
            Verifications.Add(_unityEditor.Verify("6000.0.40f1"));
            Verifications.Add(await _androidService.VerifyAdb());
            Verifications.Add(await _androidService.VerifyBaseApk(App.Current?.ModdingConfig.ApkPath ?? ""));
            
            var foundOculusPlatform = File.Exists(Path.Join(App.Current?.ModdingConfig.GamePath, "Beat Saber_Data", "Managed", "Oculus.Platform.dll"));
            
            Verifications.Add(foundOculusPlatform 
                ? new VerificationItem("Correct Beat Saber Path!", true) 
                : new VerificationItem("Invalid Beat Saber Game Path, make sure the game is Oculus version", false));

            IsVerified = Verifications.All(it => it.isVerified);
        }
        
        
        public override bool HasNavigation
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        
        public override bool CanNavigateNext
        {
            get => IsVerified;
            protected set => throw new NotSupportedException();
        }

        public override bool CanNavigatePrevious
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
    }
}