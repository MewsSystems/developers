namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client
{
    internal record ProviderExchangeRate
    {
        public ProviderExchangeRate(string currency, decimal value)
        {
            Currency = currency;
            Rate = value;
        }

        public string Currency { get; set; }

        public decimal Rate { get; set; }
    }
}
