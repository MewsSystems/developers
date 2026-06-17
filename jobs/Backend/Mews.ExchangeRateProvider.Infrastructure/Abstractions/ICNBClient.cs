using Mews.ExchangeRateProvider.Domain.Common.Dtos.CNBRates;

namespace Mews.ExchangeRateProvider.Infrastructure.Abstractions
{
    public interface ICNBClient
    {
        Task<IEnumerable<ResponseExchangeRate>> GetDailyRatesCNBAsync(string date, string lang);
    }
}
