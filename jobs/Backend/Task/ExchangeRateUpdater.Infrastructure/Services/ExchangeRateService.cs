using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Dto;
using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Services;

namespace ExchangeRateUpdater.Infrastructure.Services
{
    internal class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateProviderFactory _exchangeRateProviderFactory;

        public ExchangeRateService(IExchangeRateProviderFactory exchangeRateProviderFactory)
        {
            _exchangeRateProviderFactory = exchangeRateProviderFactory;
        }

        public async Task<ExchangeRatesResponseDto> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CurrencyCode targetCurrency)
        {
            var provider = _exchangeRateProviderFactory.Create(targetCurrency);

            var exchangeRates = await provider.GetExchangeRatesAsync(currencies, targetCurrency);

            return new ExchangeRatesResponseDto
            {
                ExchangeRates = exchangeRates
            };
        }
    }
}
