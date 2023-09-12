using ExchangeRateUpdater.Domain.Models.Enums;

namespace ExchangeRateUpdater.Domain.Providers
{
    public interface IExchangeRateProviderFactory
    {
        IExchangeRateProvider Create(CurrencyCode targetCurrency);
    }
}
