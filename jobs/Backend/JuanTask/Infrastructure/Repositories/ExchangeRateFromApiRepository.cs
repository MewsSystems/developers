using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Clients.CnbApi;
using Infrastructure.Clients.CnbApi.Basetypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ExchangeRateFromApiRepository : IExchangeRateRepository
    {

        private readonly ICnbApiClient _cnbApiClient;

        private readonly ILogger<ExchangeRateFromApiRepository> _logger;

        private readonly ExchangeRateRepositoryConfiguration _configuration;

        public ExchangeRateFromApiRepository(ICnbApiClient cnbApiClient,
                                             ILogger<ExchangeRateFromApiRepository> logger,
                                             IOptions<ExchangeRateRepositoryConfiguration> configuration)
        {
            _logger = logger;
            _cnbApiClient = cnbApiClient;
            _configuration = configuration.Value;
        }

        public async Task<IDictionary<string, ExchangeRate>> GetTodayCZKExchangeRatesDictionaryAsync()
        {

            RatesResponse apiResponse = await _cnbApiClient.GetExchangeRatesDaily(DateTimeOffset.Now);

            if (apiResponse?.Rates == null || !apiResponse.Rates.Any()) 
            { 
                return new Dictionary<string, ExchangeRate>();
            }

            Dictionary<string, ExchangeRate> exchangeRates = new Dictionary<string, ExchangeRate>();

            foreach (var rate in apiResponse.Rates)
            {

                try
                {

                    ExchangeRate exchangeRate = ExchangeRate.Create(rate.CurrencyCode,
                                                                    _configuration.TargetCurrency,
                                                                    rate.Amount,
                                                                    rate.Rate);

                    if (NotIsValidExchangeRate(exchangeRate, exchangeRates))
                    {
                        continue;
                    }

                    exchangeRates.Add(exchangeRate.SourceCurrency.Code, exchangeRate);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

            }

            return exchangeRates;


        }

        private bool NotIsValidExchangeRate(ExchangeRate exchangeRate, Dictionary<string, ExchangeRate> exchangeRates)
        {
            return exchangeRate == null ||
                   exchangeRates.ContainsKey(exchangeRate.SourceCurrency.Code);
        }

    }
}
