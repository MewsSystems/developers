using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<ExchangeRateDto>> GetDailyExchangeRateForCurrencies(CurrencyDto source, IEnumerable<CurrencyDto> targetCurrencies, CancellationToken cancellationToken);
    }
}
