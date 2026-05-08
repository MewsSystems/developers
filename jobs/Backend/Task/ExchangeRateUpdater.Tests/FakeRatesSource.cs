namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Fake implementation of IExchangeRatesSource for testing.
    /// Returns the content provided at construction time.
    /// </summary>
    public class FakeRatesSource : IExchangeRatesSource
    {
        private readonly string _content;

        public FakeRatesSource(string content)
        {
            _content = content;
        }

        public string GetLatestRatesContent() => _content;
    }
}


