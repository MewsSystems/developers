using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Types
{
    public static class CurrencyDictionary
    {
        public static IEnumerable<string> Keys => _items.Keys;
        public static IEnumerable<Currency> Values => _items.Values;

        public static bool ContainsCode(string code) 
        {
            return _items.ContainsKey(code);
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
            ["EUR"] = new Currency("EUR")
            /* TODO: Add a definite list of currencies */
        };
    }
}
