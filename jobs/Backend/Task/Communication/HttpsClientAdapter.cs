using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Communication
{
    internal class HttpsClientAdapter : IHttpsClientAdapter
    {
        public async Task<string> GetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return null;
        }
    }
}
