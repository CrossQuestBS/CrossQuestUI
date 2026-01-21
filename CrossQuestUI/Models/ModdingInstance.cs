using System;

namespace CrossQuestUI.Models
{
    public record struct ModdingInstance(string Path, string UnityPath, string Version, string GamePath, ModInfo[] mods, bool hasFirstTimeCompiled)
    {
        
    }
}