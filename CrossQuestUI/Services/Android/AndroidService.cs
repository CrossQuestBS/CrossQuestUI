using System;
using System.IO;
using Avalonia.Platform;

namespace CrossQuestUI.Services
{
    public class AndroidService : IAndroidService
    {
        public string ExtractApk(string packageId, string outputPath)
        {
            throw new System.NotImplementedException();
        }

        public bool ZipApk(string apkPath, string folder)
        {
            throw new System.NotImplementedException();
        }

        public bool SignApk(string apkPath)
        {
            throw new System.NotImplementedException();
        }

        public string GetManifest()
        {
            Uri textFileUri = new ("avares://CrossQuestUI/Assets/AndroidManifest.xml");
            using StreamReader streamReader = new (AssetLoader.Open(textFileUri));
            return streamReader.ReadToEnd();
        }
    }
}