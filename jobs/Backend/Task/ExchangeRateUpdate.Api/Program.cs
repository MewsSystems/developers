using ExchangeRateUpdater;
using ExchangeRateUpdater.RateSources.CzechNationalBank;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions<CzechNationalBankSourceOptions>()
        .BindConfiguration("CzechNationalBankOptions");

builder.Services.AddExchangeRateProvider().WithCzechNationalBankRateSource();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/get-rates", (string targetCurrency, [FromBody] List<string> rates, ExchangeRateProvider provider) =>
{
    return provider.GetLatestExchangeRates(new Currency(targetCurrency), rates.Select(x => new Currency(x)));
})
.WithName("GetRates")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
