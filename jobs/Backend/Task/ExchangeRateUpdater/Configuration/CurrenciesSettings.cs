namespace ExchangeRateUpdater.Configuration
{
    public class CurrenciesSettings
    {
        public const string SETTINGS_KEY = "CurrenciesSettings";
        public IList<string> Currencies { get; set; } = new List<string>();
    }
}
