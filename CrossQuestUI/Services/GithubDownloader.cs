using System;
using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public class GithubDownloader : IGithubDownloader
    {
        private IDownloader _downloader;

        public GithubDownloader(IDownloader downloader)
        {
            _downloader = downloader;
        }
        

        
        public async Task DownloadArchive(string username, string repository, string tag, string zipPath)
        {
            var url = $"https://github.com/{username}/{repository}/archive/refs/tags/{tag}.zip";

            await _downloader.DownloadFile(url, zipPath);
        }

        
        public async Task DownloadCommit(string username, string repository, string sha, string zipPath)
        {
            var url = $"https://github.com/{username}/{repository}/archive/{sha}.zip";
            await _downloader.DownloadFile(url, zipPath);
        }

        public async Task DownloadRawFile(string username, string repository, string branch, string filename, string path)
        {
            var url = $"https://github.com/{username}/{repository}/raw/refs/heads/{branch}/{filename}";
            await _downloader.DownloadFile(url, path);
        }
    }
}