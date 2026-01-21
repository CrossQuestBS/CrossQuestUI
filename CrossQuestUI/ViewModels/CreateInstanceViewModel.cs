using System;
using System.IO;
using System.Runtime.InteropServices.JavaScript;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CrossQuestUI.ViewModels
{
    public partial class CreateInstanceViewModel : DialogViewModel
    {
        public static string Title => "Create new instance";

        [ObservableProperty]
        private string _editorPath = "";
        
        [ObservableProperty]
        private string _questGame = "";
        
        [ObservableProperty]
        private string _gamePath = "";
        
        [ObservableProperty]
        private string _version = "";

        [ObservableProperty] private bool _hasFilledOut;

        public CreateInstanceViewModel()
        {
            PropertyChanged += (sender, args) =>
            {
                Update();
            };
        }

        public bool Update()
        {
            HasFilledOut = Directory.Exists(GamePath) && File.Exists(QuestGame) && File.Exists(EditorPath) && !String.IsNullOrEmpty(Version);
            return HasFilledOut;
        }
        
        public event EventHandler OnCreate;
        
        [RelayCommand]
        public void Create()
        {   
            if (!HasFilledOut) return;
            OnCreate(this, EventArgs.Empty);
            Close();
        }

        [RelayCommand]
        public void Cancel()
        {
            Close();
        }
        
        
    }
}