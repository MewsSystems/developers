using ExchangeRateUpdater.Api.Middlewares;
using ExchangeRateUpdater.Application.Cache;
using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure.Cache;
using ExchangeRateUpdater.Infrastructure.Common;
using ExchangeRateUpdater.Infrastructure.Common.Configuration;
using ExchangeRateUpdater.Infrastructure.CzechNationalBankExchangeRates;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ICzechNationalBankExchangeRateClient, CzechNationalBankExchangeRateClient>();
builder.Services.AddScoped<ICzechNationalBankExchangeRateClientResponseConverter, CzechNationalBankExchangeRateClientResponseConverter>();
builder.Services.AddScoped<IRestClient, RestClient>();

builder.Services.AddMediatR(mediatrConfig => mediatrConfig.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("ExchangeRateUpdater.Application")));
builder.Services.Configure<InfrastructureOptions>(builder.Configuration.GetSection(InfrastructureOptions.SectionName));
builder.Services.AddHttpClient<CzechNationalBankExchangeRateClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddScoped<IRedisClient, RedisClient>();
builder.Services.AddSingleton<IConnectionMultiplexer>(services =>
{
    var redisOptions = services.GetService<IOptions<InfrastructureOptions>>()!.Value.Redis;
    var redisConnectionString = $"{redisOptions.Url}:{redisOptions.Port},password={redisOptions.Password}";
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

await app.RunAsync();