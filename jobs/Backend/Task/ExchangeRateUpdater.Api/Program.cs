using ExchangeRateUpdater.Api;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Api.Validation;
using ExchangeRateUpdater.Contract;
using ExchangeRateUpdater.Contract.ExchangeRate;
using ExchangeRateUpdater.Lib.Exception;
using FluentValidation;
using FuncSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<CurrenciesSourceValidator>();

builder.Services.Configure<ResourcesConfiguration>(builder.Configuration.GetSection("Resources"));
builder.Services.AddSingleton<ResourcesConfiguration>(
    sp => sp.GetRequiredService<IOptions<ResourcesConfiguration>>().Value
);

builder.Services
    .AddScoped<ICurrenciesSourceValidator, CurrenciesSourceValidator>()
    .AddScoped<ICnbExchangeRateFetcher, CnbExchangeRateFetcher>()
    .AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

builder.Services.AddHttpClient<ICnbExchangeRateFetcher, CnbExchangeRateFetcher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// var currencies = new[]
// {
//     new Currency("USD"),
//     new Currency("EUR"),
//     new Currency("CZK"),
//     new Currency("JPY"),
//     new Currency("KES"),
//     new Currency("RUB"),
//     new Currency("THB"),
//     new Currency("TRY"),
//     new Currency("XYZ")
// };

app.MapGet("/exchange-rates", async (
        [FromServices] ICurrenciesSourceValidator validator,
        [FromQuery] string? currencies,
        IExchangeRateProvider provider,
        CancellationToken cancellationToken) =>
    {
        if (string.IsNullOrWhiteSpace(currencies))
        {
            return Results.BadRequest(
                ApiResponse.Failure<IEnumerable<ExchangeRate>>("Currencies query parameter is required."));
        }

        var currenciesSource = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        var validationResult = await validator.ValidateAsync(currenciesSource, cancellationToken);

        if (!validationResult.IsValid)
        {
            var message = string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage));

            return Results.BadRequest(ApiResponse.Failure<IEnumerable<ExchangeRate>>(message));
        }

        var currenciesList = currenciesSource.Select(source => new Currency(source));
        var ratesTry = await provider.GetExchangeRatesAsync(currenciesList, cancellationToken);

        return ratesTry
            .Map(
                rates => Results.Ok(ApiResponse.Successful(rates)),
                error => error switch
                {
                    GetExchangeRatesError.DataIssues => Results.Json(
                        ApiResponse.Failure<IEnumerable<ExchangeRate>>(
                            "Unable to provide a correct rates. Please try again later."),
                        statusCode: StatusCodes.Status500InternalServerError),
                    GetExchangeRatesError.ServiceUnavailable => Results.Json(
                        ApiResponse.Failure<IEnumerable<ExchangeRate>>(
                            "Source service is unavailable. Please try again later."),
                        statusCode: StatusCodes.Status503ServiceUnavailable),
                    _ => Results.Json(
                        ApiResponse.Failure<IEnumerable<ExchangeRate>>("Something went wrong. Please try again later."),
                        statusCode: StatusCodes.Status500InternalServerError)
                })
            .Get(_ => new ServiceException("Get exchange rate failed."));
    })
    .WithName("GetExchangeRates")
    .WithOpenApi();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }