using ExchangeRateUpdater.Application.Queries.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure;
using MediatR;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Api.Validators;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: "ExchangeRateUpdater"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter());

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
builder.Services.AddValidatorsFromAssemblyContaining<GetExchangeRatesRequestValidator>();

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
    IMediator mediator,
    IValidator<GetExchangeRatesRequest> validator) =>
{
    try
    {
        var request = new GetExchangeRatesRequest
        {
            CurrencyCodes = currencyCodes,
            Date = date ?? DateTime.UtcNow
        };

        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var query = new GetExchangeRatesQuery(request.ToCurrencies(), request.Date);
        var rates = await mediator.Send(query);
        
        return Results.Ok(new
        {
            Success = true,
            rates.Count,
            Rates = rates
        });
    }
    catch (ValidationException ex)
    {
        return Results.BadRequest(new
        {
            Success = false,
            Errors = ex.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            })
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