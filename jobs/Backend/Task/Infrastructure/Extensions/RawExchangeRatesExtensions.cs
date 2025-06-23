using Domain.Models;

namespace Infrastructure.Extensions;

internal static class RawExchangeRatesExtensions
{
    internal static List<ExchangeRate> ConvertToExchangeRates(this RawExchangeRates rawExchangeRates, Currency sourceCurrency)
    {
        var rates = new List<ExchangeRate>();

        foreach(var rate in rawExchangeRates.Rates)
        {
            var newRate = new ExchangeRate
                (
                    sourceCurrency,
                    rate.ConvertToCurrency(),
                    rate.Rate
                );
            rates.Add(newRate);
        }

        return rates;
    }

    private static Currency ConvertToCurrency(this RawExchangeRate rawExchangeRate)
        =>  new Currency(
                rawExchangeRate.CurrencyCode,
                rawExchangeRate.Country,
                rawExchangeRate.Currency);
}
