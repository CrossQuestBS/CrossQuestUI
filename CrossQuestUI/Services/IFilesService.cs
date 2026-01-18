using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace CrossQuestUI.Services
{
    public interface IFilesService
    {
        public Task<IStorageFile?> OpenFileAsync();
        public Task<IStorageFolder?> OpenFolderAsync();

        public void CopyFolder(string folderSrc, string folderDst);

    }
}