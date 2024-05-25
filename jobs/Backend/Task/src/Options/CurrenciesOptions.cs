namespace ExchangeRateUpdater.Options
{
    public class CurrenciesOptions
    {
        public const string CurrenciesOptionsName = "CurrenciesOptions";

        public IEnumerable<string> Currencies { get; set; }
    }
}
