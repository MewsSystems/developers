using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Tests.Unit.Helpers;

public class CzechBankExchangeRateProviderTestHelper
{
    public static List<object[]> GetData()
    {
        return new List<object[]>
        {
            new object[] { GetCurrencies() },
            new object[] { new List<Currency>() },
            new object[] { null }
        };
    }

    #region private methods

    private static List<Currency> GetCurrencies()
    {
        return new List<Currency>
        {
            new("USD"),
            new("EUR"),
            new("CZK"),
            new("JPY"),
            new("KES"),
            new("RUB"),
            new("THB"),
            new("TRY"),
            new("XYZ")
        };
    }

    #endregion
}