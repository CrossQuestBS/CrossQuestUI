using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class AndroidService(IProcessCaller processCaller, IStreamLogger logger) : IAndroidService
    {
        
        private const string AdbPathTemplate = "{0}/SDK/platform-tools/adb";
        private const string ApkSignerTemplate = "{0}/SDK/build-tools/34.0.0/apksigner";

        private string AndroidPlayer => App.Current?.ModdingConfig.AndroidPlayer ?? "";
        private string Adb => string.Format(AdbPathTemplate, AndroidPlayer);
        private string ApkSigner => string.Format(ApkSignerTemplate, AndroidPlayer);
        private string Apktool => App.Current?.ModdingConfig.Apktool ?? "";

        public async Task<bool> ExtractApk(string apkPath, string outputPath)
        {
            return await processCaller.ProcessAsync(Apktool, $"d \"{apkPath}\" -o \"{outputPath}\" -f");
        }

        public async Task<VerificationItem> VerifyAdb()
        {
            try
            {
                var success = await processCaller.ProcessAsync(Adb, "", "Android Debug Bridge version");
                return success ? new VerificationItem("Adb is verified to work as expected!", true) : new VerificationItem("Adb had different output, is it correct AndroidPlayer path?", false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new VerificationItem("Trying to run adb threw exception, is it correct AndroidPlayer path?", false);
            }
        }

        public async Task<bool> ModdedGameInstalled(string packageId)
        {
            var apkPath = await ExtractGame(packageId);
            var hasSignature = await CheckSignature(apkPath, "f8d0f840372f10dbcd6415eb8b560bc159b2647e7a0d79f3e750b0699fb0f241");
            File.Delete(apkPath);
            return hasSignature;
        }

        public bool PatchGame(string extractedBuildPath, string apkPath)
        {
            var libPath = "lib/arm64-v8a";
            var assetDataPath = "assets/bin/Data";
            
            string[] filesToCopy =
            [
                $"{assetDataPath}/boot.config", 
                $"{assetDataPath}/ScriptingAssemblies.json",
                $"{assetDataPath}/RuntimeInitializeOnLoads.json",
                $"{assetDataPath}/Managed/Metadata/global-metadata.dat"
            ]; 
            
            logger.WriteMessage("Saving AndroidManifest");
            CopyResourceFile("AndroidManifest.xml", Path.Join(apkPath, "AndroidManifest.xml"));
            
            // Copy Managed Resources files
            logger.WriteMessage("Copying Resource files");
            foreach (var file in Directory.GetFiles(Path.Join(extractedBuildPath, $"{assetDataPath}/Managed/Resources")))
            {
                File.Copy(file, Path.Join(apkPath, $"{assetDataPath}/Managed/Resources", Path.GetFileName(file)), true);
            }
            
            logger.WriteMessage("Copying lib files");
            foreach (var file in Directory.GetFiles(Path.Join(extractedBuildPath, $"{libPath}")))
            {
                File.Copy(file, Path.Join(apkPath, $"{libPath}", Path.GetFileName(file)), true);
            }

            logger.WriteMessage("Copying other files!");
            foreach (var filePath in filesToCopy)
            {
                logger.WriteMessage($"Copying file {Path.GetFileName(filePath)}");
                var extractedFilePath = Path.Join(extractedBuildPath, filePath);
                var unpackedFilePath = Path.Join(apkPath, filePath);
                File.Copy(extractedFilePath, unpackedFilePath, true);
            }

            return true;
        }

        public async Task<bool> GiveAppExternalStoragePermission(string packageId)
        {
            return await processCaller.ProcessAsync(Adb, $"shell appops set --uid {packageId} MANAGE_EXTERNAL_STORAGE allow");
        }

        public async Task<VerificationItem> VerifyBaseApk(string apkPath)
        {
            try
            {
                bool isBaseSignature = await CheckSignature(apkPath,
                    "145cd00e1b6dd98e172e40df88ed45da4bb12fc5f376c2601d61ddb65495d5d4");

                return isBaseSignature
                    ? new VerificationItem("Base APK found", true)
                    : new VerificationItem("APK signature is not from base game, make sure APK is not modded!", false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                return new VerificationItem("Exception was thrown trying to check APK signature, make sure AndroidPlayer path is set correctly", false);
            }
        }

        public async Task<bool> BuildApk(string apkPath, string folder)
        {
            return await processCaller.ProcessAsync(Apktool, $"b \"{folder}\" -o \"{apkPath}\" -f");
        }

        public async Task<bool> SignApk(string apkPath)
        {
            var tempPath = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());

            logger.WriteMessage("Creating new dir");
            Directory.CreateDirectory(tempPath);

            var keyPath = Path.Join(tempPath, "debug_key.pk8");
            var certPath = Path.Join(tempPath, "debug_cert.crt");

            logger.WriteMessage("Copying resource file!");
            CopyResourceFile("debug_key.pk8", keyPath);
            CopyResourceFile("debug_cert.crt", certPath);
            logger.WriteMessage("Signing!");
            var result = await processCaller.ProcessAsync(ApkSigner,
                $"sign -v --key \"{keyPath}\" --cert \"{certPath}\" \"{apkPath}\"");
            
            Directory.Delete(tempPath, true);
            return result;
        }

        private void CopyResourceFile(string fileName, string path)
        {
            Uri textFileUri = new ($"avares://CrossQuestUI/Assets/{fileName}");
            var stream = AssetLoader.Open(textFileUri);
            using var fileStream = File.Create(path);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(fileStream);
        }

        public async Task<bool> InstallGame(string apkPath)
        {
            return await processCaller.ProcessAsync(Adb, $"install \"{apkPath}\"");
        }

        public async Task<bool> UninstallGame(string packageId)
        {
            return await processCaller.ProcessAsync(Adb, $"uninstall {packageId}");
        }

        public async Task<string> ExtractGame(string packageId, string apkPath = "")
        {
            var output = await processCaller.ProcessOutputAsync(Adb, $"shell pm path {packageId}");

            if (apkPath == "")
                apkPath = Path.Join(Path.GetTempPath(), Guid.NewGuid() + ".apk");
            
            if (!output.Contains(packageId))
                return "";

            var path = output.Split("package:")[1];

            if (!await PullFile(path, apkPath))
            {
                return "";
            }

            return apkPath;
        }

        private async Task<bool> PullFile(string remotePath, string localPath)
        {
            return await processCaller.ProcessAsync(Adb, $"pull {remotePath} {localPath}");
        }

        private async Task<bool> MoveFiles(string src, string dst)
        {
            return await processCaller.ProcessAsync(Adb, $"shell mv {src} {dst}");
        }

        private async Task<bool> RemoveDir(string folder)
        {
            return await processCaller.ProcessAsync(Adb, $"shell rm -rf {folder}");
        }
        
        private async Task<bool> CreateDir(string folder)
        {
            return await processCaller.ProcessAsync(Adb, $"shell mkdir -p {folder}");
        }

        public async Task<bool> BackupFiles(string packageId)
        {
            logger.WriteMessage("Backing up files");
            await CreateDir($"/sdcard/Android/data/{packageId}");
            await MoveFiles($"/sdcard/Android/data/{packageId}/files", $"/sdcard/CrossQuest/{packageId}");
            
            var backupObbDir = $"/sdcard/CrossQuest/{packageId}/obb";
            var obbDir = $"/sdcard/Android/obb/{packageId}";
            
            await RemoveDir(backupObbDir);
            logger.WriteMessage("Backing up Obb");
            await MoveFiles(obbDir, backupObbDir);
            return true;
        }

        public async Task<bool> RestoreBackup(string packageId)
        {
            var backupObbDir = $"/sdcard/CrossQuest/{packageId}/obb";
            var obbDir = $"/sdcard/Android/obb/{packageId}";

            await RemoveDir(obbDir);
            await MoveFiles(backupObbDir, obbDir);
            return true;
        }

        public async Task<bool> CheckSignature(string apkPath, string expectedSha256)
        {
            return await processCaller.ProcessAsync(ApkSigner, $"verify --print-certs \"{apkPath}\"",
                $"SHA-256 digest: {expectedSha256}");
        }

        public async Task<bool> PathExists(string path)
        {
            return await processCaller.ProcessAsync(Adb, $"shell stat {path}");
        }
        
        public async Task<bool> ClearCache(string packageId)
        {
            return await processCaller.ProcessAsync(Adb, $"shell pm clear {packageId}");
        }
    }
}