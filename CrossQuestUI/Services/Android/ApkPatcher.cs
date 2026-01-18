using System;
using System.IO;

namespace CrossQuestUI.Services
{
    public class ApkPatcher : IApkPatcher
    {
        private IAdbClient _adbClient = new AdbClient();

        private IApkSigner _apkSigner = new ApkSigner();

        private readonly IAndroidService _androidService;

        public ApkPatcher(IAndroidService androidService)
        {
            _androidService = androidService;
        }
        
        public void PatchGame(string extractedBuildPath, string apkPath)
        {
            string? projectDirectory = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
            
            var findPath = "Unpacked APK path: ";

            var libPath = "lib/arm64-v8a";
            var assetDataPath = "assets/bin/Data";
            
            string[] filesToCopy =
            [
                $"{assetDataPath}/boot.config", 
                $"{assetDataPath}/ScriptingAssemblies.json",
                $"{assetDataPath}/RuntimeInitializeOnLoads.json",
                $"{assetDataPath}/RuntimeInitializeOnLoads.json",
                $"{assetDataPath}/Managed/Metadata/global-metadata.dat"
            ]; 

            File.WriteAllText(Path.Join(apkPath, "AndroidManifest.xml"), _androidService.GetManifest());
            
            // Copy Managed Resources files
            foreach (var file in Directory.GetFiles(Path.Join(extractedBuildPath, $"{assetDataPath}/Managed/Resources")))
            {
                File.Copy(file, Path.Join(apkPath, $"{assetDataPath}/Managed/Resources", Path.GetFileName(file)));
            }
            
            foreach (var file in Directory.GetFiles(Path.Join(extractedBuildPath, $"{libPath}")))
            {
                File.Copy(file, Path.Join(apkPath, $"{libPath}", Path.GetFileName(file)));
            }

            foreach (var filePath in filesToCopy)
            {
                var extractedFilePath = Path.Join(extractedBuildPath, filePath);
                var unpackedFilePath = Path.Join(apkPath, filePath);
                File.Copy(extractedFilePath, unpackedFilePath);
            }
            
            Console.WriteLine("Finished copying files, now pressing enter");
        }

        public bool ModdedGameInstalled(string packageId)
        {
           _adbClient.ExtractGame(packageId, out var apkPath);

            var hasSignature = _apkSigner.CheckSignature(apkPath, "f8d0f840372f10dbcd6415eb8b560bc159b2647e7a0d79f3e750b0699fb0f241");
            
            File.Delete(apkPath);

            return hasSignature;
        }
    }
}