using ExchangeRateUpdater.Provider.Cnb.Dtos;
using ExchangeRateUpdater.Provider.Cnb.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider.Cnb.Client
{
    public class CnbClient : ICnbClient
    {
        private readonly IOptions<CnbOptions> _cnbOptions;
        private readonly HttpClient _httpClient;

        public CnbClient(HttpClient httpClient, IOptions<CnbOptions> cnbOptions)
        {
            _cnbOptions = cnbOptions ?? throw new ArgumentNullException(nameof(cnbOptions));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CnbResponse?> GetDailyRatesAsync()
        {
            var uriBuilder = new UriBuilder(_cnbOptions.Value.BaseUrl) 
            {
                Path = _cnbOptions.Value.Paths?.DailyExrates 
            };

            using var response = await _httpClient.GetAsync(uriBuilder.Uri);

            //throw HttpRequestException if response is not successful
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<CnbResponse>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
