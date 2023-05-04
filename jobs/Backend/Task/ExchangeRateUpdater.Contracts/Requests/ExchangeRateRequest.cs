namespace ExchangeRateUpdater.Contracts.Requests;

public class ExchangeRateRequest
{
    [Required]
    public IEnumerable<Currency> Currencies { get; set; }
}
