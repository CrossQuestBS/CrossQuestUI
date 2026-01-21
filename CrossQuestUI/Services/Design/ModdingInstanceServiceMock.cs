using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class ModdingInstanceServiceMock : IModdingInstanceService
    {
        public Task<ModdingInstance[]> GetInstanceList()
        {
            throw new System.NotImplementedException();
        }

        public Task<ModdingInstance> CreateInstance(string unityPath, string version, string apkPath, string gamePath)
        {
            throw new System.NotImplementedException();
        }
    }
}