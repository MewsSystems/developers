using ExchangeRateUpdater.Api.Endpoints;
using ExchangeRateUpdater.Application.Configuration;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Infrastructure.HttpClients;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

builder.Services.Configure<ExchangeRateConfig>(
    builder.Configuration.GetSection("ExchangeRateConfig"));

builder.Services.AddSingleton<ExchangeRateConfig>(sp =>
    sp.GetRequiredService<IOptions<ExchangeRateConfig>>().Value);

builder.Services.AddHttpClient<IExchangeRateApiClient, ExchangeRateApiClient>((sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<ExchangeRateConfig>>().Value;
    client.BaseAddress = new Uri(config.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(config.Timeout);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapExchangeRatesEndpoints();

app.Run();