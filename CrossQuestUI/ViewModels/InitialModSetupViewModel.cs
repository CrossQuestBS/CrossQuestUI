using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using CrossQuestUI.Models;

namespace CrossQuestUI.ViewModels
{
    public class InitialModSetupViewModel : PageViewModelBase
    {
        public string Title => "Initial Mods Install";
        public string Message => "List over mods to first install";
        public ObservableCollection<ModInfo> Mods { get; set; } = new(new List<ModInfo>());

        public void ChangedCheckbox(string name, bool value)
        {
            var modInfo = Mods.FirstOrDefault(it => it.Name == name);

            if (value)
            {
                App.Current?.ModdingConfig.ModsInstalled.Add(modInfo);
            }
            else
            {
                App.Current?.ModdingConfig.ModsInstalled.Remove(modInfo);
            }
        }
        
        public void OnLoad()
        {
            var appPath = App.Current?.ApplicationPath;
            var modJson = Path.Join(appPath, "Downloads", "mods.json");
            if (File.Exists(modJson))
            {
                string jsonString = File.ReadAllText(modJson);

                var modInfos = JsonSerializer.Deserialize<ModInfo[]>(jsonString);

                if (modInfos is null)
                    return;
                
                foreach (var modInfo in modInfos)
                {
                    Mods.Add(modInfo);
                }
            }
            App.Current?.ModdingConfig.ModsInstalled.AddRange(Mods.Where(it => it.Required).ToList());
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