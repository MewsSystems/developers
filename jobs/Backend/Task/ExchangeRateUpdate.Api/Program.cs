using ExchangeRateUpdate.Api;
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

builder.Services.AddExchangeRateProvider().WithCzechNationalBankRateSource(useDefaultUrls: false);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/load-rates", (ExchangeRateDto dto, ExchangeRateProvider provider) =>
{
    return provider.GetLatestExchangeRates(new Currency(dto.TargetCurrency), dto.SourceCurrencies.Select(x => new Currency(x)));
})
.WithName("LoadRates")
.WithOpenApi();

app.Run();
