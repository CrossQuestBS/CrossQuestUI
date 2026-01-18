using System;
using System.Diagnostics;
using System.IO;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class UnityEditor : IUnityEditor
    {
        private readonly string _unityPath = App.Current?.ModdingConfig.UnityEditorPath ?? "";
        
        public VerificationItem Verify(string version)
        {
            try
            { 
                var startInfo = new ProcessStartInfo() { FileName = _unityPath, Arguments = " -- --headless", RedirectStandardOutput = true, CreateNoWindow = true}; 
                var proc = new Process() { StartInfo = startInfo, };
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine() ?? "";
                    Console.WriteLine(line);

                    if (!line.Contains("Unity Editor version:")) continue;
                    proc.Kill();
                    return line.Contains(version) 
                        ? new VerificationItem("Correct Unity Editor!", true) 
                        : new VerificationItem($"Incorrect Unity Version found: {line.Split("Unity Editor version: ")[1].Trim().Split("(")[0].Trim()}", false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new VerificationItem($"Failed to run Unity Editor with error: {e.Message}", false);
            }
            return new VerificationItem($"No Unity Version found, is correct executable selected?", false);
        }

        public bool CreateProject(string projectPath)
        {
            var arguments = "-batchmode ";
            arguments += $"-createProject \"{projectPath}\" ";
            arguments += "-quit -logfile - ";
    
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = _unityPath, Arguments = arguments, RedirectStandardOutput = true, CreateNoWindow = true}; 
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            proc.WaitForExit();

            return proc.ExitCode == 0;
        }
        
        public bool CompileUnityProject(string projectPath, string buildPath, string activeBuildProfile)
        {
            var arguments = "-batchmode ";
            arguments += $"-project-path \"{projectPath}\" ";
            arguments += "-quit -logfile - ";
            arguments += $"-activeBuildProfile \"{activeBuildProfile}\" ";
            arguments += $"-build \"{buildPath}\"";
    
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = _unityPath, Arguments = arguments, RedirectStandardOutput = true, CreateNoWindow = true};
            using Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                Console.WriteLine(line);
            }

            return File.Exists(buildPath);
        }
    }
}