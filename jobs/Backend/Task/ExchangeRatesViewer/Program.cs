using ExchangeRatesService.Configuration;
using ExchangeRatesService.Providers;
using ExchangeRatesService.Providers.Interfaces;
using ExchangeRatesViewer;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddOptionsWithValidateOnStart<CnbExchangeRateApiConfig>()
    .Bind(builder.Configuration.GetSection("CnbApi"))
    .ValidateDataAnnotations();

builder.Services.AddScoped<IRatesConverter, RatesConverter>();

builder.Services.AddHttpClient<ExchangeRateProvider>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<CnbExchangeRateApiConfig>>().CurrentValue;
    client.BaseAddress = new Uri(options.ApiUrl);
});

builder.Services.AddHttpClient<ForexRateProvider>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptionsMonitor<CnbExchangeRateApiConfig>>().CurrentValue;
    client.BaseAddress = new Uri(options.ApiUrl);
});

builder.Services.AddHostedService<ExchangeRatesPrinter>();

var app = builder.Build();
await app.StartAsync();