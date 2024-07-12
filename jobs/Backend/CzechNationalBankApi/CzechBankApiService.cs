using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CzechNationalBankApi
{
    /// <summary>
    /// Provides various data from the Czech bank api.
    /// https://api.cnb.cz/cnbapi/api-docs
    /// </summary>
    public class CzechBankApiService : ICzechBankApiService
    {
        private readonly ILogger<CzechBankApiService> _logger;
        private readonly HttpClient _httpClient;

        public CzechBankApiService(ILogger<CzechBankApiService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CzechExchangeItemDto>> GetExchangeRatesAsync()
        {
            var czechItems = new List<CzechExchangeItemDto>();

            //There are two endpoints to check as there are "exrates" and other "fxrates"
            var dailyResponse = await _httpClient.GetAsync($"cnbapi/exrates/daily?lang=EN&date={DateTime.UtcNow:yyyy-MM-dd}");

            dailyResponse.EnsureSuccessStatusCode();

            var dailyResponseStream = await dailyResponse.Content.ReadFromJsonAsync<CzechExchangeRatesResponseDto>() ?? new();

            czechItems.AddRange(dailyResponseStream.Rates);

            var lastMonth = DateTime.UtcNow.AddMonths(-1);

            var fxResponse = await _httpClient.GetAsync($"cnbapi/fxrates/daily-month?lang=EN&yearMonth={lastMonth:yyyy-MM}");

            fxResponse.EnsureSuccessStatusCode();

            var fxResponseStream = await fxResponse.Content.ReadFromJsonAsync<CzechExchangeRatesResponseDto>() ?? new();

            czechItems.AddRange(fxResponseStream.Rates);

            //Could have done this using hashsets instead
            var duplicates = czechItems.GroupBy(x => x.CurrencyCode.ToLower()).Where(g => g.Count() > 1);

            if (duplicates.Any())
            {
                throw new Exception("Duplicate currency code information found");
            }

            return czechItems;
        }
    }
}
