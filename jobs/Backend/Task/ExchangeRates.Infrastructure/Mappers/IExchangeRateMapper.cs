using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Models;

namespace ExchangeRates.Infrastructure.Mappers
{
    internal interface IExchangeRateMapper
    {
        IEnumerable<ExchangeRate> Map(ExRateDailyResponse exRateDailyResponse);
    }
}
