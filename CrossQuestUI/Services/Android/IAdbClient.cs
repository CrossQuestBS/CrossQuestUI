using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IAdbClient
    {
        public VerificationItem Verify();
        public bool HasQuestConnected();
        public string[] GetDevices();
        public bool UninstallGame(string packageId);
        public bool InstallGame(string packageId);
        public bool ExtractGame(string packageId, out string outputPath);
        public bool ExtractObbFiles(string packageId);

        public bool CopyObbFiles(string packageId);

        public bool ClearCache(string packageId);
    }
}