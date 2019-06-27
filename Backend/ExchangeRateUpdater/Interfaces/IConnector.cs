using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IConnector
    {
        string DownloadTxtFile(string url);
        Task<string> DownloadTxtFileAsync(string url);
    }
}