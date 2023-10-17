namespace Mews.ExchangeRateProvider.Extensions;

public sealed class ExchangeRateProviderOptions
{
    public const string Section = "ExchangeRateProvider";
    
    public IEnumerable<ExchangeRateProvider>? ExchangeRateProviders { get; init; }
}

public sealed class ExchangeRateProvider
{
    public Uri? Uri { get; init; }
}