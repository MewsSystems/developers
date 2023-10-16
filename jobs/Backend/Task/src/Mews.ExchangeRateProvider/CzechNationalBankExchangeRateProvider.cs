namespace Mews.ExchangeRateProvider;

public sealed class CzechNationalBankExchangeRateProvider : IExchangeRateProvider
{
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        throw new NotImplementedException();
    }
}