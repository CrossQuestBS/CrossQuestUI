using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public interface IDownloader
    {
        public Task DownloadFile(string url, string file);
    }
}