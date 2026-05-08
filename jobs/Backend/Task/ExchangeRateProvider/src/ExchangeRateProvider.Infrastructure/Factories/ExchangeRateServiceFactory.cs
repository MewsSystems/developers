using ExchangeRateProvider.Domain.Constants;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateProvider.Infrastructure.Factories;

public class ExchangeRateServiceFactory(IServiceProvider serviceProvider) : IExchangeRateServiceFactory
{
    public IExchangeRateService GetService(Currency baseCurrency)
    {
        return baseCurrency.Code.ToUpperInvariant() switch
        {
            CurrencyServiceKeys.CZK => serviceProvider.GetRequiredKeyedService<IExchangeRateService>(CurrencyServiceKeys.CZK),
            _ => serviceProvider.GetRequiredKeyedService<IExchangeRateService>(CurrencyServiceKeys.CZK)
        };
    }
}
