using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IUnityEditor
    {
        public VerificationItem Verify(string version);

        public Task<bool> CreateProject(string path);

        public Task<bool> CompileUnityProject(string projectPath, string buildPath, string activeBuildProfile);
    }
}