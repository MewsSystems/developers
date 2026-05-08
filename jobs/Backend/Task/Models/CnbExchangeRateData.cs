using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

public class CnbExchangeRateData
{
    public DateTime Date { get; set; }
    public List<CnbExchangeRateEntry> Rates { get; set; } = new List<CnbExchangeRateEntry>();
}

public class CnbExchangeRateEntry
{
    public string Country { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }
    public string Code { get; set; }
    public decimal Rate { get; set; }
}
