using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public interface IApkSigner
    {
        public bool CheckSignature(string apkPath, string expectedSha256);
        public bool SignApk(string apkPath, string keyPath, string certificatePath);

        public VerificationItem VerifyBaseApk(string apkPath);
    }
}