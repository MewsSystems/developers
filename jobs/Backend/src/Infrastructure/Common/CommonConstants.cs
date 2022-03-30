using ExchangeRate.Domain.Entities;

namespace ExchangeRate.Infrastructure.Common;

public static class CommonConstants
{
    public static readonly IEnumerable<Currency> Currencies = new  List<Currency>
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };
}
