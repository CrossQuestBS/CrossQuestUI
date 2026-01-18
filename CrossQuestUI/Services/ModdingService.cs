using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public class ModdingService : IModdingService
    {
        private readonly IDownloader _downloader;

        private readonly IUnityEditor _editor;

        private readonly IApkTool _apkTool;

        private readonly IApkPatcher _apkPatcher;

        private readonly IApkSigner _apkSigner;

        private readonly IAdbClient _adbClient;

        private readonly IAndroidService _androidService;

        private readonly IFilesService _filesService;

        private readonly IStreamLogger _logger;

        private string _moddingPath = "";

        private string _unityProject = "";

        private string _resourceFolder = "";

        private string _buildPath = "";
        
        private Dictionary<string, string[]> assemblies = new Dictionary<string, string[]>
        {
            {
                "beatsaber",
                [
                    "BeatGames.Analytics.dll",
                    "BeatSaber.AvatarCore.dll",
                    "BeatSaber.BeatAvatarAdapter.dll",
                    "BeatSaber.BeatAvatarSDK.dll",
                    "BeatSaber.Credits.dll",
                    "BeatSaber.Destinations.dll",
                    "BeatSaber.Environments.BTS.dll",
                    "BeatSaber.Environments.Interscope.dll",
                    "BeatSaber.Environments.LadyGaga.dll",
                    "BeatSaber.Environments.LinkinPark2.dll",
                    "BeatSaber.GameSettings.dll",
                    "BeatSaber.Init.dll",
                    "BeatSaber.Main.Leaderboards.dll",
                    "BeatSaber.Multiplayer.Core.dll",
                    "BeatSaber.Multiplayer.TimelineMock.dll",
                    "BeatSaber.OculusPlatform.dll",
                    "BeatSaber.RecPlay.dll",
                    "BeatSaber.Styles.dll",
                    "BeatSaber.Settings.dll",
                    "BeatSaber.TrackDefinitions.dll",
                    "BeatSaber.ViewSystem.dll"
                ]
            },
            {
                "bglib",
                [
                    "BGLib.AppFlow.dll",
                    "BGLib.Attributes.dll",
                    "BGLib.ClassicStaticBatcher.dll",
                    "BGLib.DotnetExtension.dll",
                    "BGLib.FileStorage.dll",
                    "BGLib.JsonExtension.dll",
                    "BGLib.MetaRemoteAssets.dll",
                    "BGLib.Notepad.dll",
                    "BGLib.Polyglot.dll",
                    "BGLib.UnityExtension.dll",
                ]
            },
            {
                "main",
                [
                    "AdditionalContentModel.Interfaces.dll",
                    "Analytics.Model.dll",
                    "BeatmapCore.dll",
                    "BGNetCore.dll",
                    "BGNetLogging.dll",
                    "Colors.dll",
                    "Core.dll",
                    "DataModels.dll",
                    "GameInit.dll",
                    "GameplayCore.dll",
                    "Helpers.dll",
                    "HMLib.dll",
                    "HMRendering.dll",
                    "HMUI.dll",
                    "Interactable.dll",
                    "LiteNetLib.dll",
                    "Main.dll",
                    "MediaLoader.dll",
                    "MenuSystem.dll",
                    "Menu.ColorSettings.dll",
                    "Menu.CommonLib.dll",
                    "MenuLightPreset.dll",
                    "MidiParser.dll",
                    "MockCore.dll",
                    "Networking.NetworkPlayerEntitlementsChecker.dll",
                    "Networking.dll",
                    "OverridableData.dll",
                    "PlatformUserModel.dll",
                    "Rendering.dll",
                    "SaberTrail.dll",
                    "SegmentedControl.dll",
                    "Transitions.dll",
                    "Tweening.dll",
                    "VRUI.dll",
                    "VRUI.Interfaces.dll",
                ]
            },
            {
                "oculus-studios",
                [
                    "OculusStudios.GraphQL.Client.dll",
                    "OculusStudios.GraphQL.ClientInterface.dll",
                    "OculusStudios.HierarchyIcons.dll",
                    "OculusStudios.Platform.Core.dll",
                    "OculusStudios.Platform.Oculus.dll"
                ]
            },
            {
                "third-party",
                [
                    "Library.UnityOpus.dll",
                    "Tayx.Graphy.dll",
                    "osce.analytics.dll",
                    "AYellowpaper.SerializedCollections.dll",
                    "BouncyCastle.Crypto.dll",
                    "Ignorance.dll",
                    "Ookii.Dialogs.dll"
                ]
            },
            {
                "unity",
                [
                    "Unity.RenderPipeline.Universal.ShaderLibrary.dll",
                    "Unity.RenderPipelines.Universal.2D.Runtime.dll",
                    "Unity.RenderPipelines.Universal.Config.Runtime.dll",
                    "Unity.RenderPipelines.Universal.Runtime.dll",
                    "Unity.RenderPipelines.Universal.Shaders.dll",
                    "Unity.Addressables.dll",
                    "Unity.Profiling.Core.dll",
                    "Unity.ResourceManager.dll",
                ]
            },
            {
                "zenject",
                [
                    "ZenjectExtension.dll",
                    "Zenject.dll",
                    "Zenject-usage.dll",
                ]
            }
        };
        
        private void CreateLinkerFile(string linkPath)
        {
            List<string> listOfEntries = [];


            foreach (var keyPair in assemblies)
            {
                listOfEntries.AddRange(keyPair.Value);
            }
            
            string[] unityFiles =
            [
                "UnityEngine.CoreModule",
                "UnityEngine.UI",
                "UnityEngine.UIModule",
                "UnityEngine.AnimationModule",
                "UnityEngine.ParticleSystemModule",
                "UnityEngine.PhysicsModule",
                "UnityEngine.VFXModule",
                "UnityEngine.XRModule",
                "UnityEngine.Timeline",
                "UnityEngine.ProBuilder",
                "UnityEngine.ProBuilder.KdTree",
                "UnityEngine.ProBuilder.Csg",
                "UnityEngine.ProBuilder.Poly2Tri",
                "UnityEngine.Mathematics"
            ]; 
            
            listOfEntries.AddRange(unityFiles);

            var linkContent = CreateLink(listOfEntries.ToArray());
            File.WriteAllText(linkPath, linkContent);
        }
        
        private string CreateLink(string[] files)
        {
            string output = "<linker>\n";
            foreach (var file in files)
            {
                output += String.Format("   <assembly fullname=\"" + file.Split("/").Last().Split(".dll").First() + "\" preserve=\"all\"/>\n");
            }

            output += "</linker>";
            return output;
        }

        private bool CopyGameFiles()
        {
            var plugins = Path.Join(_unityProject, "Assets", "Plugins");

            var pluginFolders = Directory.GetDirectories(plugins);
    
            foreach (var keyPair in assemblies)
            {
                var pluginPath = pluginFolders.First(t => t.Split("/").Last() == keyPair.Key);

                if (pluginPath == "")
                {
                    Console.WriteLine("Failed to find path for key: " + keyPair.Key);
                    continue;
                }

                foreach (var assemblyFile in keyPair.Value)
                {
                    var unityAssemblyPath = Path.Join(pluginPath, Path.GetFileName(assemblyFile));
                    if (File.Exists(unityAssemblyPath))
                        continue;
            
                    File.Copy(assemblyFile, unityAssemblyPath);
                }
            }

            return true;
        }

        public ModdingService(IDownloader downloader, IUnityEditor editor, IAndroidService androidService, IApkTool apkTool, IApkSigner apkSigner,
            IAdbClient adbClient, IStreamLogger logger, IApkPatcher patcher, IFilesService filesService)
        {
            _downloader = downloader;
            _editor = editor;
            _apkTool = apkTool;
            _apkPatcher = patcher;
            _apkSigner = apkSigner;
            _adbClient = adbClient;
            _logger = logger;
            _androidService = androidService;
            _filesService = filesService;
        }
        
        public async Task<bool> PrepareStep()
        {
            _logger.WriteMessage(" --- Preparing step --- ");
            _moddingPath = Path.Join(App.Current.ApplicationPath, "Projects", Guid.NewGuid().ToString());
            var downloadDir = Path.Join(App.Current?.ApplicationPath, "Downloads");
            _resourceFolder = Path.Join(App.Current?.ApplicationPath, "Resources", Guid.NewGuid().ToString());
            var modsDownloadDir = Path.Join(downloadDir, "Mods");

            Directory.CreateDirectory(_resourceFolder);

            var unityBase = Path.Join(downloadDir, "UnityBase.zip");

            if (!File.Exists(unityBase))
                return true;
            
            _logger.WriteMessage("Unzipping UnityBase.zip");

            await ZipFile.ExtractToDirectoryAsync(unityBase, Path.Join(_resourceFolder, "UnityBaseProject"));

            Directory.CreateDirectory(modsDownloadDir);
            
            var baseGameApk = Path.Join(_resourceFolder, "BaseGame.apk");

            
            _logger.WriteMessage($"Copying {App.Current?.ModdingConfig.ApkPath} to {baseGameApk}");
            
            File.Copy(App.Current.ModdingConfig.ApkPath, baseGameApk);
            
            _logger.WriteMessage("Installing Mods");
            foreach (var modInfo in App.Current.ModdingConfig.ModsInstalled)
            { 
                var zipFile = Path.Join(modsDownloadDir, $"{modInfo.Id}.zip");
                var modFolder = Path.Join(_resourceFolder, "Mods", $"{modInfo.Id}");
               
                _logger.WriteMessage($"Installing Mod: {modInfo.Name}");
                await _downloader.DownloadFile(modInfo.DownloadUrl, zipFile);
                await ZipFile.ExtractToDirectoryAsync(zipFile, modFolder);
            }
            _logger.WriteMessage("Done Installing Mods");
            return true;
        }

        public bool SetupUnityStep()
        {
            _logger.WriteMessage("-- Setting up Unity Project --- ");
            _logger.WriteMessage("Creating New Unity Project");
            _unityProject = Path.Join(_moddingPath, "UnityProject");

            _editor.CreateProject(_unityProject);
            
            _logger.WriteMessage("Copying Unity Base files to Unity Project ");

            string[] folders = ["Assets", "ProjectSettings", "Packages"];

            foreach (var folder in folders)
            {
                var path = Path.Join(_unityProject, folder);
                _logger.WriteMessage($"Deleting {path}");
                Directory.Delete(path);
                
                var unityBase = Path.Join(_resourceFolder, "UnityBaseProject", folder);
                _logger.WriteMessage($"Copying {unityBase} to {path}");
                _filesService.CopyFolder(unityBase, path);
            }

            _logger.WriteMessage("Copying Game Files to Unity Project ");
            CopyGameFiles();
            var plugins = Path.Join(_unityProject, "Assets", "Plugins");

            CreateLinkerFile(Path.Join(plugins, "link.xml"));
            return true;
        }

        public bool SetupModsStep()
        {
            _logger.WriteMessage("Copying Mods to UnityProject!");

            var mods = Path.Join(_unityProject, "Assets", "Plugins", "Mods");

            Directory.CreateDirectory(mods);

            var modsDirectories = Directory.GetDirectories(Path.Join(_resourceFolder, "Mods"));
    
            foreach (var directory in modsDirectories)
            {
                var directoryName = Path.GetDirectoryName(directory);
                _logger.WriteMessage($"Copying mod {directoryName} to UnityProject: {mods}");
                _filesService.CopyFolder(directory, Path.Join(mods, directoryName));
            }
            
            return true;
        }

        public bool CompileProjectStep()
        {
            _logger.WriteMessage("Starting to compile, this will take a while...");
            _buildPath = Path.Join(_moddingPath, "build", "build.apk");
            Directory.CreateDirectory(Path.Join(_moddingPath, "build"));
            return _editor.CompileUnityProject(_unityProject, Path.Join(_moddingPath, "build", "build.apk"),
                "Assets/Settings/Build Profiles/Compile.asset");
        }

        public bool PatchApkStep()
        {
            _logger.WriteMessage($"Extracting APK from buildPath: {_buildPath}");
            _androidService.ExtractApk(_buildPath, _buildPath.Replace(".apk", ""));
            
            
            _logger.WriteMessage($"Extracting APK from baseGame: {Path.Join(_resourceFolder, "BaseGame.apk")}");
            
            var apkFolder = Path.Join(_moddingPath, "BaseGame");
            _androidService.ExtractApk(Path.Join(_resourceFolder, "BaseGame.apk"), apkFolder);
            
            _logger.WriteMessage($"Patching apk: {_buildPath}");
            _apkPatcher.PatchGame(_buildPath.Replace(".apk", ""), apkFolder);

            _androidService.ZipApk(Path.Join(_moddingPath, "PatchedGame.apk"), apkFolder);
            _androidService.SignApk(Path.Join(_moddingPath, "PatchedGame.apk"));
            return true;
        }

        public bool InstallApkStep()
        {
            if (_apkPatcher.ModdedGameInstalled("com.beatgames.beatsaber"))
            {
                _adbClient.InstallGame(Path.Join(_moddingPath, "PatchedGame.apk"));
                _adbClient.ClearCache("com.beatgames.beatsaber");
            }
            else
            {
                _adbClient.ExtractObbFiles("com.beatgames.beatsaber");
                _adbClient.InstallGame(Path.Join(_moddingPath, "PatchedGame.apk"));
                _adbClient.ClearCache("com.beatgames.beatsaber");
                _adbClient.CopyObbFiles("com.beatgames.beatsaber");
            }
            return true;
        }
    }
}