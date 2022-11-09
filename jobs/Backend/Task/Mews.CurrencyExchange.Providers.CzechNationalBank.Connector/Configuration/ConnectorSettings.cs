namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Configuration
{
    public record ConnectorSettings
    {
        public string DailyFileUri { get; set; } = "";
        public string SourceCurrency { get; set; } = "";
        public string CultureInfo { get; set; } = "";
        public short CurrencyIndex { get; set; }
        public short RateIndex { get; set; }
    }
}
