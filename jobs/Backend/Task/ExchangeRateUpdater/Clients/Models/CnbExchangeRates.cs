using System.Collections.Generic;

namespace ExchangeRateUpdater.Clients.Models;

public class CnbExchangeRates
{
    public List<CnbExchangeRate> Rates { get; set; } = new();
}