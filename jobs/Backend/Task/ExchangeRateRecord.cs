using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    // Max: Rule: one class one file.  Possible exception

    public record ExchangeRatesRecord(
        IEnumerable<ExchangeRateRecord> rates
    );

    public record ExchangeRateRecord(
        string validFor,
        int order,
        string country,
        string currency,
        int amount,
        string currencyCode,
        decimal rate

    );

}
