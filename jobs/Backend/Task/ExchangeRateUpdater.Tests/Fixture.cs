using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests;

public class Fixture : IAsyncDisposable
{
    private readonly ServiceProvider _provider;

    public Fixture()
    {
        var baseAddress = "https://localhost:12345";
        var exchangeRatesEndpoint = "/rates";

        ExchangeRatesEndpointAbsoluteUrl = new UriBuilder(baseAddress)
        {
            Path = exchangeRatesEndpoint
        }.ToString();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                [$"{nameof(CzechNationalBankExchangeRateProviderOptions)}:{nameof(CzechNationalBankExchangeRateProviderOptions.BaseAddress)}"] = baseAddress,
                [$"{nameof(CzechNationalBankExchangeRateProviderOptions)}:{nameof(CzechNationalBankExchangeRateProviderOptions.ExchangeRatesEndpoint)}"] = exchangeRatesEndpoint
            })
            .Build();

        MockHttp = new MockHttpMessageHandler();

        var services = Program.GetServiceCollection(configuration);

        // Test-specific configuration to use the mock HTTP handler
        services
            .AddHttpClient(HttpClientsNames.CzechNationalBank)
            .ConfigureHttpMessageHandlerBuilder(builder => builder.PrimaryHandler = MockHttp);

        _provider = services.BuildServiceProvider();
        Sut = (CzechNationalBankExchangeRateProvider)_provider.GetRequiredService<IExchangeRateProvider>();
    }

    public CzechNationalBankExchangeRateProvider Sut { get; }
    public MockHttpMessageHandler MockHttp { get; }
    public string ExchangeRatesEndpointAbsoluteUrl { get; }

    public ValueTask DisposeAsync() => _provider.DisposeAsync();
}
