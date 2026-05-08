namespace ExchangeRateUpdater.Parsers.Models
{
    // Single exchange rate record from CNB data parser.
    public record CnbExchangeRateData(
        string Country,
        string CurrencyName,
        int Amount,
        string Code,
        decimal Rate
    );
}
