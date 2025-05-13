using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.ExternalServices;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Providers;
using ExchangeRateUpdater.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

IReadOnlyList<Currency> currencies =
        [
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        ];

IConfigurationRoot configuration = new ConfigurationBuilder()
.AddInMemoryCollection(new Dictionary<string, string?>()
{
    ["CnbExchangeRateOptions:BaseUrl"] = "https://api.cnb.cz/cnbapi/",
    ["CnbExchangeRateOptions:CommonUrl"] = "exrates/daily?lang=EN",
    ["CnbExchangeRateOptions:UncommonUrl"] = "fxrates/daily-month?lang=EN",
})
.Build();

ServiceCollection services = new();

services.AddOptions<CnbExchangeRateOptions>()
    .Bind(configuration.GetSection("CnbExchangeRateOptions"))
    .Validate((options) => !string.IsNullOrWhiteSpace(options.CommonUrl), $"Configuration error: 'CnbExchangeRateOptions:BaseUrl' must be provided")
    .Validate((options) => !string.IsNullOrWhiteSpace(options.CommonUrl), $"Configuration error: 'CnbExchangeRateOptions:CommonUrl' must be provided")
    .Validate((options) => !string.IsNullOrWhiteSpace(options.UncommonUrl), $"Configuration error: 'CnbExchangeRateOptions:UncommonUrl' must be provided")
    .ValidateOnStart();

services.AddLogging(configure =>
{
    configure.AddSimpleConsole();
    configure.AddFilter("System.Net.Http.HttpClient", LogLevel.None);
});

services.AddMemoryCache();

services.AddHttpClient<ICnbExchangeRateClient, CnbExchangeRateClient>()
.ConfigureHttpClient((serviceProvider, httpClient) =>
{
    IOptions<CnbExchangeRateOptions> cnbConfigOptions = serviceProvider.GetRequiredService<IOptions<CnbExchangeRateOptions>>();
    httpClient.BaseAddress = new Uri(cnbConfigOptions.Value.BaseUrl);
});

services.AddSingleton<IExchangeRateRepository, ExchangeRateRepository>();
services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

IExchangeRateProvider exchangeRateProvider = serviceProvider.GetRequiredService<IExchangeRateProvider>();

IEnumerable<ExchangeRate> rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");

foreach (ExchangeRate rate in rates)
{
    Console.WriteLine(rate.ToString());
}

Console.ReadLine();