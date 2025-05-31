using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddServices();

using var host = builder.Build();

var currencies = new[]
{
    new Currency("USD"),
    new Currency("EUR"),
    new Currency("CZK"),
    new Currency("JPY"),
    new Currency("KES"),
    new Currency("RUB"),
    new Currency("THB"),
    new Currency("TRY"),
    new Currency("XYZ")
};

var provider = host.Services.GetService<IExchangeRateProvider>();
var rates = provider.GetExchangeRates(currencies);

Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
foreach (var rate in rates)
{
    Console.WriteLine(rate.ToString());
}
