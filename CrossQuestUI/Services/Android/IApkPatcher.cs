namespace CrossQuestUI.Services
{
    public interface IApkPatcher
    {
        public void PatchGame(string extractedBuildPath, string apkPath);

        public bool ModdedGameInstalled(string packageId);

    }
}