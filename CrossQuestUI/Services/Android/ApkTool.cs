using System;
using System.Diagnostics;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class ApkTool : IApkTool
    {
        public VerificationItem Verify()
        {
            const string verifiedMessage = "ApkTool executable found and verified to work!";
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "apktool", Arguments = "", RedirectStandardOutput = true, UseShellExecute = true, CreateNoWindow = true}; 
                Process proc = new Process() { StartInfo = startInfo, };
                proc.Start();
     
                string line = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(line);
                proc.WaitForExit();

                if (line.Contains("apktool") && proc.ExitCode == 0)
                {
                    return new VerificationItem(verifiedMessage, true);
                }
                
                return new VerificationItem($"Unexpected failed with exitCode: {proc.ExitCode}", false);
            }
            catch (Exception e)
            {
                return new VerificationItem("Error trying to execute apktool, make sure apktool is correctly installed",
                    false);
            }
        }

        public bool ExtractApk(string apkPath)
        {
            var startInfo = new ProcessStartInfo() { FileName = "apktool", Arguments = $"d \"{apkPath}\" -f"};
            using var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();

            return proc.ExitCode == 0;
        }

        public bool ExtractApk(string apkPath, string outputPath)
        {
            var startInfo = new ProcessStartInfo() { FileName = "apktool", Arguments = $"d \"{apkPath}\" -o \"{outputPath}\" -f"};
            using var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();

            return proc.ExitCode == 0;
        }

        public bool ZipApk(string apkPath, string folder)
        {
            var startInfo = new ProcessStartInfo() { FileName = "apktool", Arguments = $"b \"{folder}\" -o \"{apkPath}\" -f"};
            using var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            proc.WaitForExit();

            return proc.ExitCode == 0;
        }
    }
}