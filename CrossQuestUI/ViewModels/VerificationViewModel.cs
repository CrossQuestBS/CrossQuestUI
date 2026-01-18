using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        
        public ObservableCollection<VerificationItem> Verifications { get; set; } = new(new List<VerificationItem>());

        public void OnUnload()
        {
            Verifications = new(new List<VerificationItem>());
        }
        
        public void OnLoad()
        {
            var editor = new UnityEditor();
            var adbClient = new AdbClient();
            var apkSigner = new ApkSigner();

            Verifications.Add(editor.Verify("6000.0.40f1"));
            Verifications.Add(adbClient.Verify());
            Verifications.Add(apkSigner.VerifyBaseApk(App.Current?.ModdingConfig.ApkPath ?? ""));
            
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