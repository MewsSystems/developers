using Domain.Abstractions.Data;
using Domain.Models;

namespace Infrastructure.Services.Data;

public class AvailableCurrencies : IAvailableCurrencies
{
    private IEnumerable<Currency> Currencies =
    [
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    ];

    public IEnumerable<Currency> GetCurrencies()
        => Currencies;

    public Currency GetCurrencyWithCode(string code)
    {
        return Currencies.FirstOrDefault(c => c.Code == code);
    }
}
