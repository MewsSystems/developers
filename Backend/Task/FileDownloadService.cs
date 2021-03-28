using System.IO;
using System.Net;

namespace ExchangeRateUpdater
{
    internal class FileDownloadService : IFileDownloadService
    {
        public string DownloadFileContent(string url)
        {
            using (var webClient = new WebClient())
            using (var stream = webClient.OpenRead(url))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}