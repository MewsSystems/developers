using ExchangeRateUpdater.Data.Interfaces;
using ExchangeRateUpdater.Data.Responses;
using ExchangeRateUpdater.Models.Models;
using Flurl;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace ExchangeRateUpdater.Data.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly IConfiguration _configuration;

        public ExchangeRateRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            ExchangeRatesResponseDto? result;

            using (var httpClient = new HttpClient())
            {
                if (string.IsNullOrEmpty(_configuration["ExchangeRateUrl"])) throw new ApplicationException("Czech National Bank Url not defined");
                httpClient.BaseAddress = new Uri(_configuration["ExchangeRateUrl"]);
                httpClient.BaseAddress
                    .SetQueryParams(new
                    {
                        date = date.ToString("yyyy-MM-dd"),
                        lang = "EN"
                    });

                var response = await httpClient.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = response.Content
                        .ReadAsStringAsync(cancellationToken)
                        .GetAwaiter().GetResult();

                    result = JsonConvert.DeserializeObject<ExchangeRatesResponseDto>(jsonResult);
                }
                else
                {
                    throw new HttpRequestException($"Failed to request ExchangeRates. Error: {response.StatusCode}");
                }
            }

            return result.Rates.Select(rate => MapToExchangeRate(rate)).ToList();
        }

        private ExchangeRate MapToExchangeRate(ExchangeRateDto exchangeRateDto)
        {
            return new ExchangeRate(
                new Currency("CZK"), //CNB only retrieve rates for CZK
                new Currency(exchangeRateDto.TargetCurrency),
                exchangeRateDto.Rate,
                exchangeRateDto.ValidFor);
        }
    }


}
