using System.Collections.Generic;

namespace ExchangeRateUpdater;

public class DailyExchangeRateResponse
{
    public IEnumerable<DailyExchangeRates>? Rates { get; set; }
}

public class DailyExchangeRates
{
    public string? CurrencyCode { get; set; }
    public int Amount { get; set; }
    public decimal Rate { get; set; }
}