namespace ExchangeRateUpdaterApi.Dtos;

/// <summary>
/// Information about a currency
/// </summary>
public class CurrencyDto
{
    /// <summary>
    /// Code associated with a currency (ex: CZK)
    /// </summary>
    public string Code { get; set; }
}