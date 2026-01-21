using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IModdingInstanceService
    {
        public Task<ModdingInstance[]> GetInstanceList();
        public Task<ModdingInstance> CreateInstance(string unityPath, string version, string apkPath, string gamePath);
    }
}