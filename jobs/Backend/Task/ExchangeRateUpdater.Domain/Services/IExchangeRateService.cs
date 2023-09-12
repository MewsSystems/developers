using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Dto;
using ExchangeRateUpdater.Domain.Models.Enums;

namespace ExchangeRateUpdater.Domain.Services
{
    public interface IExchangeRateService
    {
        public Task<ExchangeRatesResponseDto> GetExchangeRatesAsync(IEnumerable<Currency> currencies, CurrencyCode targetCurrency);
    }
}
