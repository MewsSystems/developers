using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.ValueObjects;
using LanguageExt.Common;

namespace ExchangeRateUpdater.Infrastructure.Services.Interfaces
{
    public interface IExchangeRateProvider
    {
        Result<IEnumerable<ExchangeRate>> GetExchangeRates(string scope, string? date = null, string? currency = null, IEnumerable<Currency>? currencies = null);
    }
}
