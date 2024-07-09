using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;

public class MockExchangeRatesParallelHttpClient : IExchangeRatesParallelHttpClient
{
    private readonly IEnumerable<ProviderExchangeRate> _exchangeRates;

    public MockExchangeRatesParallelHttpClient(
        IExchangeRateProviderSettings settings,
        IEnumerable<ProviderExchangeRate> exchangeRates
        )
    {
        _exchangeRates = exchangeRates;
    }


    public Task<IEnumerable<ProviderExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        return Task.FromResult(_exchangeRates);
    }
}

public class MockExchangeRateProviderSettings : IExchangeRateProviderSettings
{
    public int Precision { get; set; }

    public int MaxThreads { get; set; }

    public int RateLimitCount { get; set; }

    public int RateLimitDuration { get; set; }

    public string? SourceUrl { get; set; }
    public int TimeoutSeconds { get; set; }
}
