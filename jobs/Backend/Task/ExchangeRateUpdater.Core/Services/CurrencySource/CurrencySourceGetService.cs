using ExchangeRateUpdater.Core.Domain.Entities;
using ExchangeRateUpdater.Core.Domain.RepositoryContracts;
using ExchangeRateUpdater.Core.DTO;
using ExchangeRateUpdater.Core.ServiceContracts.CurrencySource;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Services.CurrencySource
{
    public class CurrencySourceGetService : ICurrencySourceGetService
    {
        private readonly ILogger<CurrencySourceGetService> _logger;
        private readonly ICurrencySourceRepository _currencySourceRepository;

        public CurrencySourceGetService(ILogger<CurrencySourceGetService> logger, ICurrencySourceRepository currencySourceRepository)
        {
            _logger = logger;
            _currencySourceRepository = currencySourceRepository;
        }
        public async Task<List<CurrencySourceResponse>> GetAllCurrencySources()
        {
            _logger.LogInformation("CurrencySourceGetService - GetAllCurrencySources called");

            var currencySources = await _currencySourceRepository.GetCurrencySourcesAsync();

            _logger.LogInformation("CurrencySourceGetService - currency source repository returned {CurrencySources} results", currencySources.Count());
            
            return currencySources.Select(x => x.ToCurrencySourceResponse()).ToList();
        }
    }
}
