using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Handlers;
using ExchangeRateUpdater.Api.Models;
using ExchangeRateUpdater.Infrastructure.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

var app = builder.Build();

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
    [FromServices] IValidator<ClientCredentials> validator) => 
    await AuthHandler.GenerateToken(credentials, jwtSettings, validator))
    .WithName("GenerateToken")
    .WithOpenApi();

app.MapGet("/exchange-rates", async(
    [FromQuery] string[] currencyCodes,
    [FromQuery] DateTime? date,
    [FromServices] IMediator mediator,
    [FromServices] IValidator<GetExchangeRatesRequest> validator) => 
    await ExchangeRatesHandler.GetExchangeRates(currencyCodes, date, mediator, validator))
    .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)))
    .RequireAuthorization()
    .WithName("GetExchangeRates")
    .WithOpenApi();

app.Run();