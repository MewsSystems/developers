using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;

namespace Mews.ExchangeRateProvider.Application.Abstractions
{
    public interface IRateRepository
    {
        Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(string date, string lang, bool getAllRates);
    }
}
