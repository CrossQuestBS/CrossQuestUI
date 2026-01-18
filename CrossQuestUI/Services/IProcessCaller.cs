using System.Threading.Tasks;

namespace CrossQuestUI.Services
{
    public interface IProcessCaller
    {
        public Task<bool> ProcessAsync(string fileName, string arguments);
        public Task<bool> ProcessAsync(string fileName, string arguments, string expectedOutputText);
        public Task<string> ProcessOutputAsync(string fileName, string arguments);

    }
}