using ExchangeRateUpdater.Domain.DTO;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ExchangeRateUpdater.Infrastructure.Services.Providers
{
    public class CnbExchangeService : ICnbExchangeService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string CnbDefaultLanguage = "EN";
        private readonly string CnbDateFormat = "yyyy-MM-dd";

        public CnbExchangeService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<CnbExchangeRate>> GetExchangeRatesByDateAsync(DateTime? date, CancellationToken cancellationToken)
        {
            var exRateUrl = _configuration.GetValue<string>("ExchangeRateProviders:Cnb:UrlExchangeRate");
            var queryString = $"date={date.GetValueOrDefault().ToString(CnbDateFormat)}&lang={CnbDefaultLanguage}";
            var requestUri = new Uri($"{exRateUrl}?{queryString}", UriKind.Relative);

            var response = await _client.GetAsync(requestUri, cancellationToken);
            var httpContentString = await response.Content.ReadAsStringAsync(cancellationToken);

            
            var exRates = JsonSerializer.Deserialize<CnbExchangeRates>(httpContentString, _jsonSerializerOptions);
            return exRates!.Rates;
        }
    }
}
