using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public class Downloader : IDownloader
    {
        public async Task DownloadFile(string url, string file)
        {
            using var client = new System.Net.Http.HttpClient();
            var contents = await client.GetByteArrayAsync(url);
            await System.IO.File.WriteAllBytesAsync(file, contents);
        }
    }
}