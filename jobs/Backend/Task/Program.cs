using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ExchangeRateProvider as a Scoped
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

// Add HttpClient support
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//// --- TEST --- //

//var httpClient = new HttpClient();
//var provider = new ExchangeRateProvider(httpClient);
//var currencies = new List<Currency>
//        {
//            new Currency("USD"),
//            new Currency("EUR"),
//            new Currency("CZK"),
//            new Currency("JPY"),
//            new Currency("KES"),
//            new Currency("RUB"),
//            new Currency("THB"),
//            new Currency("TRY"),
//            new Currency("XYZ")
//        };

//try
//{

//    var rates = await provider.GetExchangeRates(currencies);

//    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
//    foreach (var rate in rates)
//    {
//        Console.WriteLine($"{rate?.Amount} {rate?.SourceCurrency.Code} = {rate?.Rate} {rate?.TargetCurrency.Code}");
//    }
//}
//catch (Exception e)
//{
//    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
//}

//Console.ReadLine();