namespace CurrencyExchange.Model;

public class ExchangeRate
{
    /// <summary>
    /// Source currency of the exchange rate.
    /// </summary>
    public required Currency SourceCurrency { get; init; }

    /// <summary>
    /// Target currency of the exchange rate.
    /// </summary>
    public required Currency TargetCurrency { get; init; }

    /// <summary>
    /// Price for which one unit of the target currency is sold for in source currency.  
    /// </summary>
    public required decimal Value { get; init; }

    /// <summary>
    /// Date the rate is valid for.
    /// </summary>
    public required DateTimeOffset ValidFor { get; init; }

    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency} = {Value}";
    }
}
