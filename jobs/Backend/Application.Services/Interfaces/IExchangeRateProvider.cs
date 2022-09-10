using Domain.Model;

namespace Application.Services.Interfaces
{
    public interface IExchangeRateProvider
    {
        void LoadData();

        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);

        ExchangeRate? Convert(Currency sourceCurrency, Currency targetCurrency, int amount = 1, string referenceCurrencyCode = "CZK");
        decimal FromReferenceCurrencyCode(string currencyCode, string referenceCurrencyCode);
        decimal ToReferenceCurrencyCode(string currencyCode, string referenceCurrencyCode);
    }
}