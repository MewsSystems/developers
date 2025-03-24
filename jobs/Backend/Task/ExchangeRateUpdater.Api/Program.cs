using ExchangeRateUpdater.Application.ExchangeRates.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure;
using MediatR;
using ExchangeRateUpdater.Api.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Redis output caching
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

builder.Services.AddOutputCache()
    .AddStackExchangeRedisCache(x =>
    {
        x.InstanceName = "ExchangeRateUpdater";
        x.Configuration = redisConnectionString;
    });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

app.MapGet("/exchange-rates", async (
    [FromQuery] string[] currencyCodes,
    [FromQuery] DateTime? date,
    IMediator mediator) =>
{
    try
    {
        var request = new GetExchangeRatesRequest
        {
            CurrencyCodes = currencyCodes,
            Date = date ?? DateTime.UtcNow
        };

        var query = new GetExchangeRatesQuery(request.ToCurrencies(), request.Date);
        var rates = await mediator.Send(query);
        
        return Results.Ok(new
        {
            Success = true,
            rates.Count,
            Rates = rates
        });
    }
    catch (Exception e)
    {
        return Results.Problem(
            title: "Error fetching exchange rates",
            detail: e.Message,
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
})
.CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)))
.WithName("GetExchangeRates")
.WithOpenApi();

app.Run();