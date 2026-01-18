namespace CrossQuestUI.Services
{
    public interface IAndroidService
    {
        public string ExtractApk(string packageId, string outputPath);
        public bool ZipApk(string apkPath, string folder);

        public bool SignApk(string apkPath);

        public string GetManifest();
    }
}