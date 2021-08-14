namespace ExchangeRateUpdater
{
    public sealed record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, decimal Value)
    {
        public override string ToString()
        {
            return $"{SourceCurrency.Code}/{TargetCurrency.Code}={Value}";
        }
    }
}
