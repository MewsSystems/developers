using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Handlers;
using ExchangeRateUpdater.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthorization();

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "localhost:6379";

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ??
                  throw new InvalidOperationException("JwtSettings not found");

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

app.MapGet("/exchange-rates", ExchangeRatesHandler.GetExchangeRates)
    .CacheOutput(x => x.Expire(TimeSpan.FromMinutes(5)))
    .WithName("GetExchangeRates")
    .WithOpenApi();

app.Run();