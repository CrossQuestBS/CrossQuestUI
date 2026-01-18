using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public interface IModdingService
    {
        public Task<bool> PrepareStep();
        public bool SetupUnityStep();
        public bool SetupModsStep();
        public bool CompileProjectStep();
        public bool PatchApkStep();
        public bool InstallApkStep();
    }
}