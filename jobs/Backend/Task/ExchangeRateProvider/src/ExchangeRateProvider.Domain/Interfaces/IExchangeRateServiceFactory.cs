using ExchangeRateProvider.Domain.ValueObjects;

namespace ExchangeRateProvider.Domain.Interfaces;

public interface IExchangeRateServiceFactory
{
    IExchangeRateService GetService(Currency baseCurrency);
}
