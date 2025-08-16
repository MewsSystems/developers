using System.Collections.Generic;

namespace ExchangeRateUpdater
{
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
