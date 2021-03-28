using System.IO;

namespace ExchangeRateUpdater
{
    public interface IFileDownloadService
    {
        string DownloadFileContent(string url);
    }
}