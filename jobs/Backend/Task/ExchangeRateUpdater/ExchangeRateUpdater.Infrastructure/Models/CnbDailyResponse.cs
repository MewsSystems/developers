using System;
using System.Collections.Generic;

public class CnbDailyResponse
{
    public DateTime date { get; set; }
    public List<CnbRate> rates { get; set; }
}

public class CnbRate
{
    public string country { get; set; }
    public string currencyCode { get; set; }
    public int amount { get; set; }
    public decimal rate { get; set; }
}
