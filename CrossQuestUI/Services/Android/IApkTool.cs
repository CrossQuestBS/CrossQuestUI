using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IApkTool
    {
        public VerificationItem Verify();

        public bool ExtractApk(string apkPath);
        public bool ExtractApk(string apkPath, string outputPath);

        public bool ZipApk(string apkPath, string folder);
    }
}