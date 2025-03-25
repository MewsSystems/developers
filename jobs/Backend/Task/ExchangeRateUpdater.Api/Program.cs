using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Handlers;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Infrastructure.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

// TODO: Integrate Serilog (or similar) for logging and configure OpenTelemetry sink

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthorization();

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString") 
    ?? throw new InvalidOperationException("Redis:ConnectionString is required");

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() 
    ?? throw new InvalidOperationException("JwtSettings configuration is required");

builder.Services.AddOpenTelemetry(builder.Environment.ApplicationName);
builder.Services.AddDistributedRedisCache(builder.Environment.ApplicationName, redisConnectionString);
builder.Services.AddValidators();
builder.Services.AddMediatr();
builder.Services.AddInfrastructure();

builder.Services.AddJwtBearerAuthentication(jwtSettings);
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminAccess", policy => policy.RequireRole("Admin"));
    });

var app = builder.Build();

// TODO: Populate Swagger or use configuration/openapi.yaml to generate Postman collection
// TODO: From .NET 9 onwards, refer to https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/token", async (
    [FromBody] ClientCredentials credentials,
    [FromServices] IValidator<ClientCredentials> validator,
    CancellationToken cancellationToken) => 
    await AuthHandler.GenerateToken(credentials, jwtSettings, validator, cancellationToken))
    .WithName("GenerateToken")
    .WithOpenApi();

app.MapGet("/exchange-rates", async(
    [FromQuery] string[] currencyCodes,
    [FromQuery] DateTime? date,
    [FromServices] IMediator mediator,
    [FromServices] IValidator<GetExchangeRatesRequest> validator,
    CancellationToken cancellationToken) => 
    await ExchangeRatesHandler.GetExchangeRates(currencyCodes, date, mediator, validator, cancellationToken))
    .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)).Tag("exchange-rates"))
    .RequireAuthorization()
    .WithName("GetExchangeRates")
    .WithOpenApi();

app.MapPost("/cache/purge", async (
    IOutputCacheStore cache,
    CancellationToken cancellationToken) =>
    await CacheHandler.PurgeCache(cache, cancellationToken))
    .RequireAuthorization("AdminAccess")
    .WithName("PurgeCache")
    .WithOpenApi();

app.Run();