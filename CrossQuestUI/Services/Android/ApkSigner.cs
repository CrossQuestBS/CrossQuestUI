using System;
using System.Diagnostics;
using System.IO;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class ApkSigner : IApkSigner
    {
        private readonly string _signerPath = Path.Join(App.Current?.ModdingConfig.AndroidPlayer, "SDK", "build-tools", "34.0.0", "apksigner");

        public bool CheckSignature(string apkPath, string expectedSha256)
        {
            var startInfo = new ProcessStartInfo() { FileName = _signerPath, Arguments = $"verify --print-certs \"{apkPath}\"", RedirectStandardOutput = true, CreateNoWindow = true}; 
            var proc = new Process() { StartInfo = startInfo, };
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine() ?? "";
                Console.WriteLine(line);

                if (!line.Contains("SHA-256 digest:")) continue;
                proc.Kill();
                return line.Contains(expectedSha256);
            }

            return false;
        }

        public bool SignApk(string apkPath, string keyPath, string certificatePath)
        {
            var startInfo = new ProcessStartInfo() { FileName = _signerPath, Arguments = $"sign --key \"{keyPath}\" --cert \"{certificatePath}\" \"{apkPath}\"", RedirectStandardOutput = true, CreateNoWindow = true}; 
            var proc = new Process() { StartInfo = startInfo, };
            proc.Start();
            proc.WaitForExit();
            
            return proc.ExitCode == 0;
        }

        public VerificationItem VerifyBaseApk(string apkPath)
        {
            
            try
            {
                bool isBaseSignature = CheckSignature(apkPath,
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
    }
}