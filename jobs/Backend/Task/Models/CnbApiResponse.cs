using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

public class CnbApiResponse
{
    public List<CnbExchangeRate> Rates { get; set; } = new();
}

public class CnbExchangeRate
{
    // Include only properties that are needed for calculations.
    public int Amount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
}
