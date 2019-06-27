using ExchangeRateUpdater.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Connector : IConnector
    {
        public async Task<string> DownloadTxtFileAsync(string url)
        {
            // Having HttpClient wrapped in using is wrong approach, see: https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            // but for just for simplicity i will leave it like that, because static variable could cause problems if application is multithread
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage result = await httpClient.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }

        public string DownloadTxtFile(string url)
        {
            // I hate doing stuff like this, but unfortunatelly HttpClient doesn't have synchronous functions, 
            // so i hope this will do less harm than other solutions ( simple `Result` or `GetAwaiter().GetResult()` )
            // https://stackoverflow.com/a/32429753
            string content = Task.Run(() => DownloadTxtFileAsync(url)).Result;
            return content;
        }
    }
}
