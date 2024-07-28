using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts.ExchangeRate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Services.ExchangeRate
{
    public class ExchangeRatesGetService : IExchangeRateGetService
    {
        private readonly ILogger<ExchangeRatesGetService> _logger;
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ExchangeRatesGetService(ILogger<ExchangeRatesGetService> logger, IExchangeRateRepository exchangeRateRepository)
        {
            _logger = logger;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates()
        {
            _logger.LogInformation("GetExchangeRates of ExchangeRatesGetService called");

            var exchangeRates = await _exchangeRateRepository.GetExchangeRatesAsync();

            _logger.LogInformation("Exchange Rate Repository returned {exchangeRates} results", exchangeRates.Count());

            return exchangeRates.Select(rate => rate.ToExchangeRateResponse()).ToList();
        }

        public async Task<IEnumerable<ExchangeRateResponse>> GetFilteredExchangeRates(List<string> currencyCodes)
        {
            _logger.LogInformation("GetFilteredExchangeRates of ExchangeRatesGetService called");
            IEnumerable<ExchangeRateResponse> exchangeRates = await GetExchangeRates();

            var filteredExchangeRates = exchangeRates.Where(e => currencyCodes.Any(c => e.TargetCurrency.ToUpper() == c.ToUpper()));

            _logger.LogInformation("Exchange Rate Repository returned {exchangeRates} results", exchangeRates.Count());

            return filteredExchangeRates;
        }
    }
}
