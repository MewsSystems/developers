using System.Text.Json;
using ErrorOr;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Infrastructure
{
    public class CzechNationalBankApiClient : IExchangeRateProviderRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CzechNationalBankApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ErrorOr<IEnumerable<ExchangeRate>>> GetCentralBankRates(string exchangeRateDate, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("CzechNationalBankApi");

            try
            {
                var response = await httpClient.GetAsync($"exrates/daily?date={exchangeRateDate}&lang=EN");

                if (!response.IsSuccessStatusCode)
                {
                    return Error.Unexpected(description: "Unable to call Czech National Bank API");
                }

                var content = await response.Content.ReadAsStringAsync();
                var ratesResponse = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);

                if (ratesResponse == null)
                {
                    return Error.Unexpected(description: "Unable to deserialize API response");
                }

                if (ratesResponse.Rates != null)
                {
                    return ratesResponse.Rates.Select(rate => new ExchangeRate(new Currency(rate.CurrencyCode),
                        new Currency("CZK"), decimal.Divide(rate.Rate, rate.Amount))).ToList();
                }

                return new List<ExchangeRate>();               
            }
            catch (Exception ex)
            {
                return Error.Unexpected(description: ex.Message);
            }

        }
    }

}
