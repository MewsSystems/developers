using System.Net.Http.Json;
using ExchangeRateUpdated.CnbApi.Model;
using ExchangeRateUpdater.Model;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.CnbApi
{
    
    public class CnbApiClient : ICnbApiClient
    {
        private readonly HttpClient httpClient;

        private readonly CnbApiOptions _config;

        public CnbApiClient(HttpClient httpClient, IOptions<CnbApiOptions> config)
        {
            this.httpClient = httpClient;
            _config = config.Value;
        }

        /// <inheritdoc/>
        public async Task<ICollection<ExchangeRate>> GetDailyRates(CancellationToken cancellationToken)
        {
            var resp = await httpClient.GetFromJsonAsync<ExRateDailyResponse>(_config.ApiUrl, cancellationToken);
            return resp.Rates.Select(rate => rate.ToExchangeRate()).ToList();
        }
    }
}
