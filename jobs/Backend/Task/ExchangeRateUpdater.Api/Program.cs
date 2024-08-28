using ExchangeRateUpdater.Api.Middlewares;
using ExchangeRateUpdater.Application.GetExchangeRates;
using ExchangeRateUpdater.Infrastructure.Common;
using ExchangeRateUpdater.Infrastructure.Common.Configuration;
using ExchangeRateUpdater.Infrastructure.CzechNationalBankExchangeRates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ICzechNationalBankExchangeRateClient, CzechNationalBankExchangeRateClient>();
builder.Services.AddScoped<ICzechNationalBankExchangeRateClientResponseConverter, CzechNationalBankExchangeRateClientResponseConverter>();
builder.Services.AddScoped<IRestClient, RestClient>();

builder.Services.AddMediatR(mediatrConfig => mediatrConfig.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("ExchangeRateUpdater.Application")));
builder.Services.Configure<InfrastructureOptions>(builder.Configuration.GetSection(InfrastructureOptions.SectionName));
builder.Services.AddHttpClient<CzechNationalBankExchangeRateClient>().SetHandlerLifetime(TimeSpan.FromMinutes(5));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

await app.RunAsync();