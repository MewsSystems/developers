namespace ExchangeRateUpdater.Options
{
    public class CurrenciesOptions
    {
        public const string OptionsName = "CurrenciesOptions";

        public IEnumerable<string> Currencies { get; set; }
    }
}
