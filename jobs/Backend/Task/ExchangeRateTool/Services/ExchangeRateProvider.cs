using CnbServiceClient.Interfaces;
using CnbServiceClient.DTOs;
using ExchangeEntities;
using ExchangeRateTool.Interfaces;
using Microsoft.Extensions.Configuration;
using Utils.Extensions;

namespace ExchangeRateTool.Services
{
	public class ExchangeRateProvider : IExchangeRateProvider
	{
        private readonly IExratesService _exrateService;
        private readonly IExrateFilterService _exrateFilterService;
        private readonly IExchangeRateFactory _exchangeRateFactory;
        private readonly IConfiguration _configuration;

        public ExchangeRateProvider(IExratesService exrateService, IExrateFilterService exrateFilterService, IExchangeRateFactory exchangeRateFactory, IConfiguration configuration)
        {
            _exrateService = exrateService;
            _exrateFilterService = exrateFilterService;
            _exchangeRateFactory = exchangeRateFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            if (!currencies.Any())
            {
                return new List<ExchangeRate>();
            }

            // Get all the exrates
            var apiExrates = await _exrateService.GetExratesDailyAsync();

            // Filter exrates with the desired currencies
            var desiredExrates = _exrateFilterService.Filter(apiExrates, currencies);

            // Get ExchangeRate list from Exrates
            var exchangeRates = GetExchangeRatesFromExrates(desiredExrates);

            return exchangeRates;
        }

        private IEnumerable<ExchangeRate> GetExchangeRatesFromExrates(IEnumerable<Exrate> exrates)
        {
            var exchangeRates = new List<ExchangeRate>();

            foreach (var exrate in exrates)
            {
                exchangeRates.Add(_exchangeRateFactory.Build(exrate.CurrencyCode, _configuration.GetRequiredValue<string>("TargetCurrencyCode"), exrate.Rate));
            }

            return exchangeRates;
        }
    }
}

