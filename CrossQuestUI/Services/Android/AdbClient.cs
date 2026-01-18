using System;
using System.Diagnostics;
using System.IO;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class AdbClient : IAdbClient
    {
        private readonly string _adbPath = Path.Join(App.Current?.ModdingConfig.AndroidPlayer, "SDK", "platform-tools", "adb");

        public VerificationItem Verify()
        {
            try
            {
                var startInfo = new ProcessStartInfo() { FileName = _adbPath, Arguments = "", RedirectStandardOutput = true, CreateNoWindow = true}; 
                using var proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();

                if (proc.StandardOutput.ReadToEnd().Contains("Android Debug Bridge version"))
                {
                    return new VerificationItem("Adb is verified to work as expected!", true);
                }

                return new VerificationItem("Adb had different output, is it correct AndroidPlayer path?", false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new VerificationItem("Trying to run adb threw exception, is it correct AndroidPlayer path?", false);
            }
        }

        public bool HasQuestConnected()
        {
            try
            {
                var startInfo = new ProcessStartInfo() { FileName = _adbPath, Arguments = "devices -l", RedirectStandardOutput = true, CreateNoWindow = true};
                using var proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
     
                string line = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(line);
                return line.Contains("model:Quest");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public string[] GetDevices()
        {
            throw new System.NotImplementedException();
        }

        public bool UninstallGame(string packageId)
        {
            throw new System.NotImplementedException();
        }

        public bool InstallGame(string packageId)
        {
            throw new System.NotImplementedException();
        }

        public bool ExtractGame(string packageId, out string outputPath)
        {
            throw new NotImplementedException();
        }

        public bool ExtractGame(string packageId, string outputPath)
        {
            var startInfo = new ProcessStartInfo() { FileName = _adbPath, Arguments = "", RedirectStandardOutput = true, CreateNoWindow = true}; 
            using var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            
            proc.WaitForExit();
            return proc.ExitCode == 0;
        }

        public bool CopyObbFiles(string packageId)
        {
            throw new NotImplementedException();
        }

        public bool ClearCache(string packageId)
        {
            var startInfo = new ProcessStartInfo() { FileName = _adbPath, Arguments = $"shell pm clear {packageId}", RedirectStandardOutput = true, CreateNoWindow = true}; 
            using var proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            proc.WaitForExit();
            return proc.ExitCode == 0;
        }

        public bool ExtractObbFiles(string packageId)
        {
            throw new NotImplementedException();
        }
    }
}