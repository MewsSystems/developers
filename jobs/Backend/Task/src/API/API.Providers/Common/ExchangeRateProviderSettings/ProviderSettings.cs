namespace Provider.Settings
{
    public class ProviderSettings
    {
        public ProviderSettings(string baseAddress, string exchangeRateEndpoint, int httpClientTimeout)
        {
            BaseAddress = baseAddress;
            ExchangeRateEndpoint = exchangeRateEndpoint;
            HttpClientTimeout = httpClientTimeout;
        }

        public string BaseAddress { get; }
        public string ExchangeRateEndpoint { get; }
        public int HttpClientTimeout { get; }
    }
}
