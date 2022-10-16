namespace ExchangeRateUpdater.Clients.Cnb.Models;

public class ExchangeRate
{
    public string Country { get; set; }

    public string Currency { get; set; }

    public int Amount { get; set; }

    public string Code { get; set; }

    public decimal Rate { get; set; }
}