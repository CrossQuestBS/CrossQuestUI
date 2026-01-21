using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IModdingService
    {
        public Task<bool> PrepareStep();
        public Task<bool> SetupUnityStep();
        public bool SetupModsStep();
        public Task<bool> CompileProjectStep();
        public Task<bool> PatchApkStep();
        public Task<bool> InstallApkStep();

        public Task<bool> RunProcess(bool initial, ModdingInstance instance);
    }
}