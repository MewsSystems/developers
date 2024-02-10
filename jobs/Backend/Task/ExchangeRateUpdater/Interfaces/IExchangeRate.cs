namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRate
    {
        public ICurrency SourceCurrency { get; }

        public ICurrency TargetCurrency { get; }

        public decimal Value { get; }
    }
}
