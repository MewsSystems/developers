using ExchangeRateUpdater.CnbProvider.Abstractions;
using ExchangeRateUpdater.CnbProvider.CnbClientResponses;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider
{
    public class CnbRateProviderClient : ICnbRateProviderClient
    {
        public async Task<IEnumerable<CnbRateResponseDto>> GetRatesByDateAsync(string url)
        {
            IEnumerable<CnbRateResponseDto> cbnDailyRates = Enumerable.Empty<CnbRateResponseDto>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    cbnDailyRates = JsonConvert.DeserializeObject<CnbRatesResponseDto>(apiResponse).Rates;
                }
            }

            return cbnDailyRates;
        }
    }
}
