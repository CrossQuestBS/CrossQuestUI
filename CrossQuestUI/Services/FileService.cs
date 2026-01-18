using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace CrossQuestUI.Services
{
    public class FilesService : IFilesService
    {
        public async Task<IStorageFile?> OpenFileAsync()
        {
            var files = await App.Current.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open File",
                AllowMultiple = false
            });

            return files.Count >= 1 ? files[0] : null;
        }

        public async Task<IStorageFolder?> OpenFolderAsync()
        {
            var folders = await App.Current.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Open Folder",
                AllowMultiple = false
            });

            return folders.Count >= 1 ? folders[0] : null;
        }

        public void CopyFolder(string folderSrc, string folderDst)
        {
            Directory.CreateDirectory(folderDst);
            
            foreach (var file in Directory.GetFiles(folderSrc))
            {
                var dest = Path.Combine(folderDst, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            string[] folders = Directory.GetDirectories(folderSrc);
            foreach (var folder in folders)
            {
                var dest = Path.Combine(folderDst, Path.GetDirectoryName(folder) ?? "");
                CopyFolder(folder, dest);
            }
        }
    }
}