using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CrossQuestUI.Models;

namespace CrossQuestUI.Services
{
    public class UnityEditor : IUnityEditor
    {
        private string UnityPath => App.Current?.ModdingConfig.UnityEditorPath ?? "";

        private readonly IStreamLogger _logger;

        private readonly IProcessCaller _processCaller;
        
        public UnityEditor(IStreamLogger streamLogger, IProcessCaller processCaller)
        {
            _logger = streamLogger;
            _processCaller = processCaller;
        }
        
        public VerificationItem Verify(string version)
        {
            try
            { 
                var startInfo = new ProcessStartInfo() { FileName = UnityPath, Arguments = " -- --headless", RedirectStandardOutput = true, CreateNoWindow = true}; 
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

        public async Task<bool> CreateProject(string projectPath)
        {
            var arguments = "-batchmode ";
            arguments += $"-createProject \"{projectPath}\" ";
            arguments += "-quit -logfile - ";

            _logger.WriteMessage("Creating New Unity Project");
            return await _processCaller.ProcessAsync(UnityPath, arguments);
        }
        
        public async Task<bool> CompileUnityProject(string projectPath, string buildPath, string activeBuildProfile)
        {
            var arguments = "-batchmode ";
            arguments += $"-project-path \"{projectPath}\" ";
            arguments += "-quit -logfile - ";
            arguments += $"-activeBuildProfile \"{activeBuildProfile}\" ";
            arguments += $"-build \"{buildPath}\"";
    
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = UnityPath, Arguments = arguments, CreateNoWindow = true};
            using Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();

            await proc.WaitForExitAsync();

            return File.Exists(buildPath);
        }
    }
}