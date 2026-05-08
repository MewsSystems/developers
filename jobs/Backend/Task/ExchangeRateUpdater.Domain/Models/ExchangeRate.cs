namespace ExchangeRateUpdater.Domain.Models;

public record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value, DateOnly Date)
{
    public override string ToString()
    {
        return $"{SourceCurrency}/{TargetCurrency}={Value} (Date: {Date:yyyy-MM-dd})";
    }
}
