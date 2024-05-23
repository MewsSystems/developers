using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRateDto>> GetDailyExchangeRateForCurrencies(CurrencyDto target, IEnumerable<CurrencyDto> currencies, CancellationToken cancellationToken);
    }
}
