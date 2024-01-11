using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Implementations.CzechNationalBank;

class ExchangeRateProviderCzechNationalBank : IExchangeRateProvider
{
    public GetExchangeRates GetExchangeRates => GetExchangeRatesImpl;

    public IEnumerable<ExchangeRate> GetExchangeRatesImpl(IEnumerable<Currency> currencies)
    {
        return new ExchangeRate[] {
            new ExchangeRate(new Currency("USD"), new Currency("BTC"), 24)
        };
    }
}