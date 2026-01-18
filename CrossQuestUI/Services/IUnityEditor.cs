using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IUnityEditor
    {
        public VerificationItem Verify(string version);

        public bool CreateProject(string path);

        public bool CompileUnityProject(string projectPath, string buildPath, string activeBuildProfile);
    }
}