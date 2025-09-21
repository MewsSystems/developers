using Exchange.Api.Dtos;
using Exchange.Api.Middlewares;
using Exchange.Application.Extensions;
using Exchange.Application.Services;
using Exchange.Domain.ValueObjects;
using Exchange.Infrastructure.Extensions.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDateTimeProvider();
builder.Services.AddCnbApiClient(builder.Configuration);
builder.Services.AddInMemoryCache(builder.Configuration);
builder.Services.AddExchangeRateProvider();

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/exchange-rates", async (
        [FromServices] IExchangeRateProvider exchangeRateProvider,
        [FromQuery] string[] currencyCodes,
        CancellationToken cancellationToken
    ) =>
    {
        var requestedCurrencies = currencyCodes.Select(Currency.FromCode).ToList();

        var exchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(requestedCurrencies, cancellationToken);

        return exchangeRates.Select(er =>
            new ExchangeRateDto(
                er.SourceCurrency.Code,
                er.TargetCurrency.Code,
                er.Value)
        ).ToList();
    })
    .WithOpenApi();

await app.RunAsync();