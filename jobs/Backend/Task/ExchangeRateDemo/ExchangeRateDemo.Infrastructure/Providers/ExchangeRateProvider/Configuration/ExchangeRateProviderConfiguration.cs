namespace ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ExchangeRateProviderConfiguration
    {
        public const string Name = "ExchangeRateProvider";

        public required Uri Endpoint { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}
