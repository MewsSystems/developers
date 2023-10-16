using Mews.ExchangeRateUpdater.Dtos;

namespace Mews.ExchangeRateUpdater.Services
{
    public interface IExchangeRateProviderService
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(List<CurrencyDto> currencies);
    }
}
