using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public interface IGithubDownloader
    {
        public Task DownloadArchive(string username, string repository, string tag, string zipPath);
        public Task DownloadCommit(string username, string repository, string sha, string zipPath);
        public Task DownloadRawFile(string username, string repository, string branch, string filename, string path);
    }
}