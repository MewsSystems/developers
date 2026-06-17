using ExchangeRateUpdater.Domain.Types;

namespace ExchangeRateUpdater.Application.Services
{
    public interface IExchangeRateProviderService
    {
        Task<NonNullResponse<Dictionary<string, ExchangeRate>>> GetExchangeRates();
    }
}