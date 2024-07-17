namespace ExchangeRateUpdater.Models;

/// <param name="Amount">I'm not sure if this means '100 of a currency compared to 1 in CZB' or if it means '100 of a currency compared to 100 in CZB'. I didn't see anything on the website. If this was a real project I would probably rely on a PM for the answer.</param>
/// <param name="Order">I'm not sure what this number means.</param>
/// <param name="ValidFor">This is used to determine how long to cache the value for.</param>
public record DailyExRateItem
(
    int Amount,
    string Country,
    string Currency,
    string CurrencyCode,
    int Order,
    decimal Rate,
    DateTime ValidFor
);
