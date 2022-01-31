using System.Globalization;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateParser
    {
        ExchangeRateParseResult ParseExchangeRate(string text);
        public int ParseAmount(string amount);
        public decimal NormalizeRate(int amount, decimal rate);
        public decimal ParseRate(string rate,
            NumberStyles numberStyles = NumberStyles.Any,
            CultureInfo culture = null) =>
            decimal.Parse(rate, numberStyles, culture ?? CultureInfo.InvariantCulture);
    }
}