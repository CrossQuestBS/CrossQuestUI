using System.Collections.Generic;

namespace CrossQuestUI.Models
{
    public record struct ModdingConfig(string UnityEditorPath, string ApkPath, string GamePath, string AndroidPlayer, List<ModInfo> ModsInstalled)
    {
    }
}