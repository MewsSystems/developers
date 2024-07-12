using Microsoft.Extensions.Logging;

namespace CzechNationalBankApi
{
    /// <summary>
    /// Provides various data from the Czech bank api.
    /// https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/index.html?date=
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
            //Two stage check - encapsulate within one method
            //  Some currencies are main currencies, then there are "other" ones. Just check both inside the method and do out best to return something :-)
            //      https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=11.07.2024
            //  Second check, and final: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=2024&month=6
            //All values from the two api endpoints come back like: Country|Currency|Amount|Code|Rate

            throw new NotImplementedException();
        }

    }
}
