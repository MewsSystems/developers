namespace ExchangeRateUpdater.Models
{
    public record Currency(string Code)
    {
        public override string ToString() => Code;
    }
}
