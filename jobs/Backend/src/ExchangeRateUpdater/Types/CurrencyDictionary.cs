using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Types
{
    public static class CurrencyDictionary
    {
        public static IEnumerable<string> Keys => _items.Keys;
        public static IEnumerable<Currency> Values => _items.Values;

        public static bool ContainsCode(string code) 
        {
            return _items.ContainsKey(code.ToUpper());
        }

        private static Dictionary<string, Currency> _items = new Dictionary<string, Currency>()
        {
            ["CZK"] = new Currency("CZK"),
            ["GBP"] = new Currency("GBP"),
            ["USD"] = new Currency("USD"),
            ["JPY"] = new Currency("JPY"),
            ["KES"] = new Currency("KES"),
            ["RUB"] = new Currency("RUB"),
            ["THB"] = new Currency("THB"),
            ["EUR"] = new Currency("EUR"),
            ["SGD"] = new Currency("SGD"),
            ["CHF"] = new Currency("CHF"),
            ["SEK"] = new Currency("SEK"),
            ["KRW"] = new Currency("KRW"),
            ["ZAR"] = new Currency("ZAR"),
            ["PLN"] = new Currency("PLN"),
            ["RON"] = new Currency("RON"),
            ["PHP"] = new Currency("PHP"),
            ["NOK"] = new Currency("NOK"),
            ["NZD"] = new Currency("NZD"),
            ["MXN"] = new Currency("MXN"),
            ["MYR"] = new Currency("MYR"),
            ["AUD"] = new Currency("AUD")
            /* TODO: Add a definite list of currencies */
        };
    }
}
