namespace ExchangeRateUpdater.Domain.Parsers;

public class ExchangeRatesParser : IExchangeRatesParser
{
    private const string SourceCurrency = "CZK";

    public IEnumerable<ExchangeRate> Parse(string text)
    {
        IEnumerable<ExchangeRate> exchangeRates = Enumerable.Empty<ExchangeRate>();
        var exchangeRatesRows = text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                    .Skip(2);
        var exchangeRatesValues = exchangeRatesRows.Select(x => x.Split('|')).ToList();
        foreach (var exchangeRateValues in exchangeRatesValues)
        {
            var exchangeRate = new ExchangeRate(new Currency(exchangeRateValues[3]),
                                                new Currency(SourceCurrency),
                                                int.Parse(exchangeRateValues[2]),
                                                decimal.Parse(exchangeRateValues[4], CultureInfo.InvariantCulture));

            exchangeRates = exchangeRates.Append(exchangeRate);
        }

        return exchangeRates;
    }
}
