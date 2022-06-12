namespace Mews.BackendDeveloperTask.ExchangeRates.Cnb;

public class CnbTextExchangeRateParser : ICnbTextExchangeRateParser
{
    public IEnumerable<ExchangeRate> Parse(string text)
    {
        using var stringReader = new StringReader(text);
        string? line;
        var lineCount = 0;
        while ((line = stringReader.ReadLine()) != null)
        {
            if (++lineCount <= 2) // Skip date and header lines
            {
                continue;
            }
            var parts = line.Split("|");
            if (parts.Length < 5
              || !decimal.TryParse(parts[2], out var sourceUnits)
              || !decimal.TryParse(parts[4], out var targetUnits))
            {
                continue; // The text is not valid, skip this row rather than cause exceptions
            }

            var sourceCurrency = Enum.Parse<Currency>(parts[3]);
            var targetCurrency = Currency.CZK;
            var rate = targetUnits / sourceUnits;
            yield return new ExchangeRate(sourceCurrency, targetCurrency, rate);
        }
    }
}