using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IAndroidService
    {
        public Task<bool> ExtractApk(string apkPath, string outputPath);
        public Task<bool> BuildApk(string apkPath, string folder);
        public Task<bool> SignApk(string apkPath);
        public Task<bool> InstallGame(string apkPath);
        public Task<bool> UninstallGame(string packageId);
        public Task<string> ExtractGame(string packageId, string apkPath = "");
        public Task<bool> BackupFiles(string packageId);
        public Task<bool> RestoreBackup(string packageId);
        public Task<bool> ClearCache(string packageId);
        public Task<bool> CheckSignature(string apkPath, string expectedSha256);
        public Task<VerificationItem> VerifyBaseApk(string apkPath);
        public Task<VerificationItem> VerifyAdb();

        public Task<bool> ModdedGameInstalled(string packageId);

        public bool PatchGame(string extractedBuildPath, string apkPath);
    }
}